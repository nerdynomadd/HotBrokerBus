using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Abstractions.Stan.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using STAN.Client;

namespace HotBrokerBus.Stan.Events
{
    public class StanBusEventRegister : IStanBusEventRegister
    {
        private const string AUTOFAC_SCOPE_NAME = "nats_event_bus";

        private readonly IStanBusPersistentConnection _stanBusPersistentConnection;

        private readonly IStanBusSubscriptionsStorage _stanBusSubscriptionsStorage;

        private readonly IStanBusEventStorage _eventStorage;

        private readonly ILifetimeScope _lifetimeScope;

        private readonly ILogger<StanBusEventRegister> _logger;

        private readonly Dictionary<string, IStanSubscription> _subscriptions;

        public StanBusEventRegister(IStanBusPersistentConnection stanBusPersistentConnection,
            IStanBusSubscriptionsStorage stanBusSubscriptionsStorage,
            IStanBusEventStorage eventStorage,
            ILifetimeScope lifetimeScope,
            ILogger<StanBusEventRegister> logger)
        {
            _stanBusPersistentConnection = stanBusPersistentConnection;

            _stanBusSubscriptionsStorage = stanBusSubscriptionsStorage;

            _eventStorage = eventStorage;

            _lifetimeScope = lifetimeScope;

            _logger = logger;

            _subscriptions = new Dictionary<string, IStanSubscription>();

            Connection = _stanBusPersistentConnection.CreateModel();
        }

        public IStanConnection Connection { get; set; }

        void IStanBusEventRegister.Resume()
        {
            // Delete the internal subscriptions registry
            _subscriptions.Clear();

            foreach (var subscription in _stanBusSubscriptionsStorage.RetrieveEventSubscriptions())
            {
                var type = subscription.SubscriptionDescriber;
                var type2 = subscription.SubscriptionDescriberHandler;

                typeof(StanBusEventRegister)
                    .GetMethod(nameof(Subscribe),
                        new Type[] {typeof(string), typeof(string), typeof(string), typeof(StanSubscriptionOptions)})?
                    .MakeGenericMethod(type, type2)
                    .Invoke(this, new object[]
                    {
                        subscription.Subject,
                        subscription.TriggerName,
                        subscription.QueueGroup,
                        subscription.SubscriptionOptions
                    });
            }

            foreach (var message in _eventStorage.RetrieveEventMessages())
            {
                typeof(StanBusEventRegister)
                    .GetMethod(nameof(Publish), new Type[] {typeof(string), typeof(string), typeof(IEvent)})?
                    .Invoke(this, new object[]
                    {
                        message.Item1,
                        message.Item2,
                        message.Item3
                    });
            }
            
            _eventStorage.ClearMessages();
        }

        void IStanBusEventRegister.SetConnection(IStanConnection connection)
        {
            Connection = connection;
        }

        public void Publish(string subject, IEvent @event)
        {
            Publish(subject, @event.GetType().Name, @event);
        }

        public void Publish(string subject, string eventName, IEvent @event)
        {
            var publishName = $"{subject}.{eventName}";

            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
            {
                _eventStorage.AddEventMessage(subject, eventName, @event);

                return;
            }

            var jsonMessage = JsonConvert.SerializeObject(@event);

            Connection.Publish(publishName, Encoding.UTF8.GetBytes(jsonMessage));
        }

        public void Subscribe<T, TH>(string subject, string queueGroup)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            Subscribe<T, TH>(subject, queueGroup, StanSubscriptionOptions.GetDefaultOptions());
        }

        public void Subscribe<T, TH>(string subject, string queueGroup, StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            Subscribe<T, TH>(subject, typeof(T).Name, queueGroup, subscriptionOptions);
        }
        
        public void Subscribe<T, TH>(string subject, string eventName, string queueGroup)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            Subscribe<T, TH>(subject, eventName, queueGroup, StanSubscriptionOptions.GetDefaultOptions());
        }

        public void Subscribe<T, TH>(string subject, string eventName, string queueGroup,
            StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            var subscriptionName = $"{subject}.{eventName}";

            if (!_stanBusSubscriptionsStorage.HasSubscription(subscriptionName))
                _stanBusSubscriptionsStorage.AddEventSubscription<T, TH>(subscriptionName, subject, eventName,
                    queueGroup, subscriptionOptions);

            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                return;

            _subscriptions.Add(subscriptionName, Connection.Subscribe(subscriptionName, queueGroup, subscriptionOptions,
                async (sender, args) =>
                {
                    using (var scope = _lifetimeScope.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                    {
                        var middlewareStorage = scope.ResolveOptional<IStanBusMiddlewareStorage>();

                        if (middlewareStorage == null) return;
                        
                        var middlewareComponent = middlewareStorage.GetEventMiddlewares().First.Value;

                        if (middlewareComponent == null) return;

                        var middlewareExecutionContext = new StanBusEventExecutionContext(middlewareComponent,
                            subscriptionName,
                            args.Message.Data,
                            typeof(T),
                            null, 
                            typeof(TH),
                            null,
                            args,
                            _lifetimeScope);

                        await middlewareComponent.Process(middlewareExecutionContext);
                    }
                }));
        }

        public void Close<T>(string subject)
            where T : IEvent
        {
            Close(subject, typeof(T).Name);
        }

        public void Close(string subject, string eventName)
        {
            var subscriptionName = $"{subject}.{eventName}";

            if (_subscriptions.ContainsKey(subscriptionName)) _subscriptions[subscriptionName].Close();
        }

        public void CloseAll()
        {
            foreach (var subscription in _subscriptions) subscription.Value.Close();
        }

        public void Unsubscribe<T>(string subject)
            where T : IEvent
        {
            Unsubscribe(subject, typeof(T).Name);
        }

        public void Unsubscribe(string subject, string eventName)
        {
            var subscriptionName = $"{subject}.{eventName}";

            _stanBusSubscriptionsStorage.RemoveSubscription(subscriptionName);

            if (_subscriptions.ContainsKey(subscriptionName)) _subscriptions[subscriptionName].Unsubscribe();
        }

        public void UnsubscribeAll()
        {
            foreach (var subscription in _subscriptions)
            {
                _stanBusSubscriptionsStorage.RemoveSubscription(subscription.Key);

                subscription.Value.Unsubscribe();
            }
        }
    }
}