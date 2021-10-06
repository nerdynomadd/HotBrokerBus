using System.Text;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Events;
using Newtonsoft.Json;
using STAN.Client;

namespace HotBrokerBus.Stan.Events
{
	public class StanEventBusConsumerClient : IStanEventBusConsumerClient
	{
		private readonly IStanBusPersistentConnection _stanBusPersistentConnection;

		private readonly IStanEventBusStorage _storage;

		public StanEventBusConsumerClient(IStanBusPersistentConnection stanBusPersistentConnection,
			IStanEventBusStorage storage)
		{
			_stanBusPersistentConnection = stanBusPersistentConnection;
			
			_storage = storage;

			Connection = _stanBusPersistentConnection.CreateModel();
		}

		public IStanConnection Connection { get; set; }
		
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
	}
}