using HotBrokerBus.Abstractions.Events;

namespace HotBrokerBus.Abstractions.Middleware.Events
{
    public interface IEventBusMiddleware<in T> : IBusMiddleware<T>
        where T: IEventExecutionContext
    {
        
    }
}