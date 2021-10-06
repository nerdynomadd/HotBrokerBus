using HotBrokerBus.Abstractions.Commands;

namespace HotBrokerBus.Abstractions.Middleware.Commands
{
    public interface ICommandBusMiddleware<in T> : IBusMiddleware<T>
        where T: ICommandBusExecutionContext
    {
        
    }
}