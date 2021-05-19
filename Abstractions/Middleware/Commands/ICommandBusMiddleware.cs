using HotBrokerBus.Abstractions.Commands;

namespace HotBrokerBus.Abstractions.Middleware.Commands
{
    public interface ICommandBusMiddleware : IBusMiddleware<ICommandExecutionContext>
    {
        
    }
}