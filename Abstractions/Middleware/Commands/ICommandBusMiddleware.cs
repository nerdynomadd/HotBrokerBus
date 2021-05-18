using Bus.Abstractions.Commands;

namespace Bus.Abstractions.Middleware.Commands
{
    public interface ICommandBusMiddleware : IBusMiddleware<ICommandExecutionContext>
    {
        
    }
}