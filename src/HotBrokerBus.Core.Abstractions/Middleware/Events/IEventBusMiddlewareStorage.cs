using HotBrokerBus.Abstractions.Events;

namespace HotBrokerBus.Abstractions.Middleware.Events
{
    public interface IEventBusMiddlewareStorage<out TExecutionContext> : IBusMiddlewareStorage<IEventBusMiddleware<TExecutionContext>>
        where TExecutionContext: IEventExecutionContext
    {
        
    }
}