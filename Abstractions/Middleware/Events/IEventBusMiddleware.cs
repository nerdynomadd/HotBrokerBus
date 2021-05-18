using Bus.Abstractions.Events;

namespace Bus.Abstractions.Middleware.Events
{
    public interface IEventBusMiddleware : IBusMiddleware<IEventExecutionContext>
    {
        
    }
}