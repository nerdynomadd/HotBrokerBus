using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Abstractions.Stan.Events;

namespace HotBrokerBus.Abstractions.Stan.Middleware
{
    public interface IStanEventBusMiddlewareStorage : IEventBusMiddlewareStorage<IStanEventBusExecutionContext>
    {
        
    }
}