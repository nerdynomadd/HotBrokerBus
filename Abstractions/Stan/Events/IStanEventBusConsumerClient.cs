using HotBrokerBus.Abstractions.Events;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Events
{
	public interface IStanEventBusConsumerClient
	{
		public IStanConnection Connection { get; set; }
		
		void Publish(string subject, IEvent @event);
	}
}