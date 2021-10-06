using HotBrokerBus.Abstractions.Middleware;

namespace HotBrokerBus.Abstractions
{
    public interface IBusExecutionContext
    {
        IBusMiddlewareComponent MiddlewareComponent { get; }
    }
}