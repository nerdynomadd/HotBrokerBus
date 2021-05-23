using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Middleware;
using Microsoft.Extensions.Logging;
using NATS.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STAN.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HotBrokerBus.Stan.Commands
{
    public class StanBusCommandRegister : IStanBusCommandRegister
    {
        private const string AUTOFAC_SCOPE_NAME = "nats_command_bus";

        private readonly ILifetimeScope _lifetimeScope;

        private readonly IStanBusPersistentConnection _stanBusPersistentConnection;

        private readonly IStanBusSubscriptionsStorage _stanBusSubscriptionsStorage;

        private readonly ILogger<StanBusCommandRegister> _logger;

        private readonly Dictionary<string, IAsyncSubscription> _subscriptions;

        public StanBusCommandRegister(IStanBusPersistentConnection stanBusPersistentConnection,
            IStanBusSubscriptionsStorage stanBusSubscriptionsStorage,
            ILifetimeScope lifetimeScope,
            ILogger<StanBusCommandRegister> logger)
        {
            _stanBusPersistentConnection = stanBusPersistentConnection;

            _stanBusSubscriptionsStorage = stanBusSubscriptionsStorage;

            _lifetimeScope = lifetimeScope;

            _logger = logger;

            _subscriptions = new Dictionary<string, IAsyncSubscription>();

            Connection = _stanBusPersistentConnection.CreateModel();
        }

        public IStanConnection Connection { get; set; }

        void IStanBusCommandRegister.Resume()
        {
            // Delete the internal subscriptions registry
            _subscriptions.Clear();

            foreach (var subscription in _stanBusSubscriptionsStorage.RetrieveCommandSubscriptions())
            {
                var type = subscription.SubscriptionDescriber;
                var type2 = subscription.SubscriptionDescriberHandler;

                typeof(StanBusCommandRegister)
                    .GetMethod(nameof(Subscribe), new Type[] {typeof(string), typeof(string)})?
                    .MakeGenericMethod(type, type2)
                    .Invoke(this, new object[]
                    {
                        subscription.Subject,
                        subscription.TriggerName
                    });
            }
        }

        void IStanBusCommandRegister.SetConnection(IStanConnection connection)
        {
            Connection = connection;
        }

        public TIntegrationCommandResult Send<TIntegrationCommandResult>(string subject,
            ICommand<TIntegrationCommandResult> command)
            where TIntegrationCommandResult : ICommandResult
        {
            return Send(subject, command.GetType().Name, command);
        }

        public TIntegrationCommandResult Send<TIntegrationCommandResult>(string subject, string commandName,
            ICommand<TIntegrationCommandResult> command)
            where TIntegrationCommandResult : ICommandResult
        {
            var requestName = $"{subject}.{commandName}";
            var jsonMessage = JsonConvert.SerializeObject(command);

            var msg = Connection.NATSConnection.Request(requestName, Encoding.UTF8.GetBytes(jsonMessage));

            return JsonSerializer.Deserialize<TIntegrationCommandResult>(
                Encoding.UTF8.GetString(msg.Data));
        }

        public Task SendAsync(string subject,
            ICommand command,
            int timeout = 5000)
        {
            return SendAsync(subject, command.GetType().Name, command, timeout);
        }

        public async Task SendAsync(string subject,
            string commandName,
            ICommand command,
            int timeout = 5000)
        {
            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                Connection = _stanBusPersistentConnection.CreateModel();

            var requestName = $"{subject}.{commandName}";
            var jsonMessages = JsonConvert.SerializeObject(command);

            await Connection.NATSConnection.RequestAsync(requestName, Encoding.UTF8.GetBytes(jsonMessages),
                timeout);
            
        }

        public Task<TIntegrationCommandResult> SendAsync<TIntegrationCommandResult>(string subject,
            ICommand<TIntegrationCommandResult> command,
            int timeout = 5000)
            where TIntegrationCommandResult : ICommandResult
        {
            return SendAsync(subject, command.GetType().Name, command, timeout);
        }

        public async Task<TIntegrationCommandResult> SendAsync<TIntegrationCommandResult>(string subject,
            string commandName,
            ICommand<TIntegrationCommandResult> command,
            int timeout = 5000)
            where TIntegrationCommandResult : ICommandResult
        {
            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                Connection = _stanBusPersistentConnection.CreateModel();

            var requestName = $"{subject}.{commandName}";
            var jsonMessages = JsonConvert.SerializeObject(command);

            var msg = await Connection.NATSConnection.RequestAsync(requestName, Encoding.UTF8.GetBytes(jsonMessages),
                timeout);

            return JsonSerializer.Deserialize<TIntegrationCommandResult>(Encoding.UTF8.GetString(msg.Data));
        }

        public void Subscribe<T, TH>(string subject)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>
        {
            Subscribe<T, TH>(subject, typeof(T).Name);
        }

        public void Subscribe<T, TH>(string subject, string commandName)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>
        {
            var subscriptionName = $"{subject}.{commandName}";

            if (!_stanBusSubscriptionsStorage.HasSubscription(subscriptionName))
                _stanBusSubscriptionsStorage.AddCommandSubscription<T, TH>(subscriptionName, subject, commandName);

            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                return;

            _subscriptions.Add(subscriptionName, Connection.NATSConnection.SubscribeAsync(subscriptionName,
                async (sender, args) =>
                {
                    using (var scope = _lifetimeScope.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                    {
                        var middlewareStorage = scope.ResolveOptional<IStanBusMiddlewareStorage>();

                        if (middlewareStorage == null) return;
                        
                        var middlewareComponent = middlewareStorage.GetCommandMiddlewares().First.Value;

                        if (middlewareComponent == null) return;
                        
                        var middlewareExecutionContext = new StanBusCommandExecutionContext(middlewareComponent,
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

        public void Unsubscribe<T, TH>(string subject)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>(string subject, string eventName) where T : ICommand<ICommandResult> where TH : ICommandHandler<T, ICommandResult>
        {
            var subscriptionName = $"{subject}.{eventName}";

            _stanBusSubscriptionsStorage.RemoveSubscription(subscriptionName);

            if (_subscriptions.ContainsKey(subscriptionName)) _subscriptions[subscriptionName].Unsubscribe();        }
    }
}