using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Abstractions.Stan.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using STAN.Client;

namespace HotBrokerBus.Stan.Events
{
    public class StanEventBusSubscriberClient : IStanEventBusSubscriberClient
    {
        private readonly IStanBusPersistentConnection _stanBusPersistentConnection;

        private readonly IStanBusSubscriptionsStorage _stanBusSubscriptionsStorage;

        private readonly IStanEventBusStorage _storage;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<StanEventBusSubscriberClient> _logger;

        private readonly Dictionary<string, IStanSubscription> _subscriptions;

        public StanEventBusSubscriberClient(IStanBusPersistentConnection stanBusPersistentConnection,
            IStanBusSubscriptionsStorage stanBusSubscriptionsStorage,
            IStanEventBusStorage storage,
            IServiceProvider serviceProvider,
            ILogger<StanEventBusSubscriberClient> logger)
        {
            _stanBusPersistentConnection = stanBusPersistentConnection;

            _stanBusSubscriptionsStorage = stanBusSubscriptionsStorage;

            _storage = storage;

            _serviceProvider = serviceProvider;

            _logger = logger;

            _subscriptions = new Dictionary<string, IStanSubscription>();

            Connection = _stanBusPersistentConnection.CreateModel();
        }

        public IStanConnection Connection { get; set; }

        void IStanEventBusSubscriberClient.Resume()
        {
            // Delete the internal subscriptions registry
            _subscriptions.Clear();

            foreach (var subscription in _stanBusSubscriptionsStorage.RetrieveEventSubscriptions())
            {
                var type = subscription.SubscriptionDescriber;
                var type2 = subscription.SubscriptionDescriberHandler;

                typeof(StanEventBusSubscriberClient)
                    .GetMethod(nameof(Subscribe),
                        new Type[] {typeof(string), typeof(string), typeof(StanSubscriptionOptions)})?
                    .MakeGenericMethod(type, type2)
                    .Invoke(this, new object[]
                    {
                        subscription.Subject,
                        subscription.QueueGroup,
                        subscription.SubscriptionOptions
                    });
            }

            foreach (var message in _storage.RetrieveEventMessages())
            {
                typeof(StanEventBusSubscriberClient)
                    .GetMethod(nameof(Publish), new Type[] {typeof(string), typeof(string), typeof(IEvent)})?
                    .Invoke(this, new object[]
                    {
                        message.Item1,
                        message.Item2
                    });
            }
            
            _storage.ClearMessages();
        }

        void IStanEventBusSubscriberClient.SetConnection(IStanConnection connection)
        {
            Connection = connection;
        }

        public void Publish(string subject, IEvent @event)
        {
            var publishName = $"{subject}";

            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
            {
                _storage.AddEventMessage(subject, @event);

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

        public void Subscribe<T, TH>(string subject, string queueGroup,
            StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            var subscriptionName = $"{subject}";

            if (!_stanBusSubscriptionsStorage.HasSubscription(subscriptionName))
                _stanBusSubscriptionsStorage.AddEventSubscription<T, TH>(subscriptionName, subject,
                    queueGroup, subscriptionOptions);

            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                return;

            _subscriptions.Add(subscriptionName, Connection.Subscribe(subscriptionName, queueGroup, subscriptionOptions,
                async (sender, args) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    
                    var middlewareStorage = _serviceProvider.GetService<IStanEventBusMiddlewareStorage>();
                        
                    var middlewareComponent = middlewareStorage?.GetMiddlewares()?.First?.Value;

                    if (middlewareComponent == null) return;

                    var middlewareExecutionContext = new StanEventBusExecutionContext(middlewareComponent,
                        subscriptionName,
                        args.Message.Data,
                        typeof(T),
                        null, 
                        typeof(TH),
                        null,
                        args,
                        _serviceProvider);

                    await middlewareComponent.Process(middlewareExecutionContext);
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