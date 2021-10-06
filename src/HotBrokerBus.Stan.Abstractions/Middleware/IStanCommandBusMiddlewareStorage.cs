using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Stan.Commands;

namespace HotBrokerBus.Abstractions.Stan.Middleware
{
    public interface IStanCommandBusMiddlewareStorage : ICommandBusMiddlewareStorage<IStanCommandBusExecutionContext>
    {
        
    }
}