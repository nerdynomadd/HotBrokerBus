using System.Threading.Tasks;

namespace HotBrokerBus.Abstractions.Events
{
    public interface IEventHandler<in TIntegrationEvent> : IEventHandler
        where TIntegrationEvent : IEvent
    {
        Task<bool> Handle(TIntegrationEvent @event);
    }

    public interface IEventHandler
    {
    }
}