using HotBrokerBus.Abstractions.Events;

namespace HotBrokerBus.Abstractions.Middleware.Events
{
    public interface IEventBusMiddleware : IBusMiddleware<IEventExecutionContext>
    {
        
    }
}