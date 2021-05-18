using Bus.Middleware;

namespace Bus.Abstractions
{
    public interface IBusExecutionContext
    {
        BusMiddlewareComponent MiddlewareComponent { get; }
    }
}