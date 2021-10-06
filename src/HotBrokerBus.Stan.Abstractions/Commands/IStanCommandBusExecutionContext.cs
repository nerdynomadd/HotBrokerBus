using HotBrokerBus.Abstractions.Commands;
using NATS.Client;

namespace HotBrokerBus.Abstractions.Stan.Commands
{
	public interface IStanCommandBusExecutionContext : ICommandBusExecutionContext
	{
		
		public MsgHandlerEventArgs BusArguments { get; set; }
		
	}
}