using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Middleware;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;

namespace HotBrokerBus.Abstractions.Stan.Middleware
{
    public delegate Task InternalStanEventMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next,
        IStanEventBusExecutionContext context);

    public delegate Task InternalStanCommandMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next,
        IStanCommandBusExecutionContext context);
}