using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NATS.Client;
using Newtonsoft.Json;
using STAN.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HotBrokerBus.Stan.Commands
{
    public class StanCommandBusRegister : IStanCommandBusRegister
    {
        private readonly IStanBusPersistentConnection _stanBusPersistentConnection;

        private readonly IStanBusSubscriptionsStorage _stanBusSubscriptionsStorage;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<StanCommandBusRegister> _logger;

        private readonly Dictionary<string, IAsyncSubscription> _subscriptions;

        public StanCommandBusRegister(IStanBusPersistentConnection stanBusPersistentConnection,
            IStanBusSubscriptionsStorage stanBusSubscriptionsStorage,
            IServiceProvider serviceProvider,
            ILogger<StanCommandBusRegister> logger)
        {
            _stanBusPersistentConnection = stanBusPersistentConnection;

            _stanBusSubscriptionsStorage = stanBusSubscriptionsStorage;

            _serviceProvider = serviceProvider;

            _logger = logger;

            _subscriptions = new Dictionary<string, IAsyncSubscription>();

            Connection = _stanBusPersistentConnection.CreateModel();
        }

        public IStanConnection Connection { get; set; }

        void IStanCommandBusRegister.Resume()
        {
            // Delete the internal subscriptions registry
            _subscriptions.Clear();

            foreach (var subscription in _stanBusSubscriptionsStorage.RetrieveCommandSubscriptions())
            {
                var type = subscription.SubscriptionDescriber;
                var type2 = subscription.SubscriptionDescriberHandler;

                typeof(StanCommandBusRegister)
                    .GetMethod(nameof(Subscribe), new Type[] {typeof(string)})?
                    .MakeGenericMethod(type, type2)
                    .Invoke(this, new object[]
                    {
                        subscription.Subject
                    });
            }
        }

        void IStanCommandBusRegister.SetConnection(IStanConnection connection)
        {
            Connection = connection;
        }

        public TIntegrationCommandResult Send<TIntegrationCommandResult>(string subject,
            ICommand<TIntegrationCommandResult> command)
            where TIntegrationCommandResult : ICommandResult
        {
            var requestName = $"{subject}";
            var jsonMessage = JsonConvert.SerializeObject(command);

            var msg = Connection.NATSConnection.Request(requestName, Encoding.UTF8.GetBytes(jsonMessage));

            return JsonSerializer.Deserialize<TIntegrationCommandResult>(
                Encoding.UTF8.GetString(msg.Data));
        }

        public async Task SendAsync(string subject,
            ICommand command,
            int timeout = 5000)
        {
            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                Connection = _stanBusPersistentConnection.CreateModel();

            var requestName = $"{subject}";
            var jsonMessages = JsonConvert.SerializeObject(command);

            await Connection.NATSConnection.RequestAsync(requestName, Encoding.UTF8.GetBytes(jsonMessages),
                timeout);
            
        }

        public async Task<TIntegrationCommandResult> SendAsync<TIntegrationCommandResult>(string subject,
            ICommand<TIntegrationCommandResult> command,
            int timeout = 5000)
            where TIntegrationCommandResult : ICommandResult
        {
            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                Connection = _stanBusPersistentConnection.CreateModel();

            var requestName = $"{subject}";
            var jsonMessages = JsonConvert.SerializeObject(command);

            var msg = await Connection.NATSConnection.RequestAsync(requestName, Encoding.UTF8.GetBytes(jsonMessages),
                timeout);

            return JsonSerializer.Deserialize<TIntegrationCommandResult>(Encoding.UTF8.GetString(msg.Data));
        }

        public void Subscribe<T, TH>(string subject)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>
        {
            var subscriptionName = $"{subject}";

            if (!_stanBusSubscriptionsStorage.HasSubscription(subscriptionName))
                _stanBusSubscriptionsStorage.AddCommandSubscription<T, TH>(subscriptionName, subject);

            if (Connection == null || Connection.NATSConnection == null || Connection.NATSConnection.IsClosed() ||
                Connection.NATSConnection.IsReconnecting())
                return;

            _subscriptions.Add(subscriptionName, Connection.NATSConnection.SubscribeAsync(subscriptionName,
                async (sender, args) =>
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var middlewareStorage = _serviceProvider.GetService<IStanCommandBusMiddlewareStorage>();

                        if (middlewareStorage == null) return;
                        
                        var middlewareComponent = middlewareStorage?.GetMiddlewares()?.First?.Value;

                        if (middlewareComponent == null) return;
                        
                        var middlewareExecutionContext = new StanCommandBusExecutionContext(middlewareComponent,
                            subscriptionName,
                            args.Message.Data,
                            typeof(T),
                            null, 
                            typeof(TH),
                            null,
                            args,
                            _serviceProvider);
                        
                        await middlewareComponent.Process(middlewareExecutionContext);
                    }
                }));
        }
        
        public void Unsubscribe<T, TH>(string subject) where T : ICommand<ICommandResult> where TH : ICommandHandler<T, ICommandResult>
        {
            var subscriptionName = $"{subject}";

            _stanBusSubscriptionsStorage.RemoveSubscription(subscriptionName);

            if (_subscriptions.ContainsKey(subscriptionName)) _subscriptions[subscriptionName].Unsubscribe();        }
    }
}