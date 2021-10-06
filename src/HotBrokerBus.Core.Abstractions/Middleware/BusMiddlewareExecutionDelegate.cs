using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;

namespace HotBrokerBus.Abstractions.Middleware
{
    public delegate Task BusMiddlewareExecutionDelegate(IBusExecutionContext busExecutionContext);

    public delegate Task InternalEventMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next, IEventExecutionContext context);

    public delegate Task InternalCommandMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next, ICommandBusExecutionContext context);
}