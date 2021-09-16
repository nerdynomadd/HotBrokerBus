using System.Threading.Tasks;
using HotBrokerBus.Middleware;

namespace HotBrokerBus.Abstractions.Middleware
{
    public interface IBusMiddleware<T> where T: IBusExecutionContext
    {
        public Task Invoke(BusMiddlewareExecutionDelegate next, T context);
    }
}