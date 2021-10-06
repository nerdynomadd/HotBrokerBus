using HotBrokerBus.Abstractions.Events;
using NATS.Client;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Events
{
    public interface IStanEventBusExecutionContext : IEventExecutionContext
    {
        public StanMsgHandlerArgs BusArguments { get; }
    }
}