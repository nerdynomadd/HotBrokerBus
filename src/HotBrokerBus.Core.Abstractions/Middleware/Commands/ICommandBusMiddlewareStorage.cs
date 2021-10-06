using HotBrokerBus.Abstractions.Commands;

namespace HotBrokerBus.Abstractions.Middleware.Commands
{
    public interface ICommandBusMiddlewareStorage<out TExecutionContext> : IBusMiddlewareStorage<ICommandBusMiddleware<TExecutionContext>>
        where TExecutionContext: ICommandBusExecutionContext
    {
        
    }
}