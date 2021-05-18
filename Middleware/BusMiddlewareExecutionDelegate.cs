using System.Threading.Tasks;
using Bus.Abstractions;
using Bus.Abstractions.Commands;
using Bus.Abstractions.Events;

namespace Bus.Middleware
{
    public delegate Task BusMiddlewareExecutionDelegate(IBusExecutionContext busExecutionContext);

    internal delegate Task InternalEventMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next, IEventExecutionContext context);

    internal delegate Task InternalCommandMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next, ICommandExecutionContext context);
}