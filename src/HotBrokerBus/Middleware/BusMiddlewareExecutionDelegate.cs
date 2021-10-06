using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;

namespace HotBrokerBus.Middleware
{
    public delegate Task BusMiddlewareExecutionDelegate(IBusExecutionContext busExecutionContext);
    
    [ReferenceAssembly: InternalsVisibleTo("HotBrokerBus.Stan")]
    internal delegate Task InternalEventMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next, IEventExecutionContext context);

    internal delegate Task InternalCommandMiddlewareExecutionDelegate(BusMiddlewareExecutionDelegate next, ICommandExecutionContext context);
}