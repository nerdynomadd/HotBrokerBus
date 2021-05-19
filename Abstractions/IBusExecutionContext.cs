using HotBrokerBus.Middleware;

namespace HotBrokerBus.Abstractions
{
    public interface IBusExecutionContext
    {
        BusMiddlewareComponent MiddlewareComponent { get; }
    }
}