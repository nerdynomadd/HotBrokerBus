using System.Text;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using STAN.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HotBrokerBus.Stan.Commands
{
	public class StanCommandBusConsumerClient : IStanCommandBusConsumerClient
    {
        private readonly IStanBusPersistentConnection _stanBusPersistentConnection;

        private readonly ILogger<StanCommandBusSubscriberClient> _logger;

        public StanCommandBusConsumerClient(IStanBusPersistentConnection stanBusPersistentConnection,
            ILogger<StanCommandBusSubscriberClient> logger)
        {
            _stanBusPersistentConnection = stanBusPersistentConnection;
            
            _logger = logger;
            
            Connection = stanBusPersistentConnection.CreateModel();
        }

        public IStanConnection Connection { get; set; }
        
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
	}
}