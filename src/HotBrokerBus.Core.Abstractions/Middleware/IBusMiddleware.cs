using System.Threading.Tasks;

namespace HotBrokerBus.Abstractions.Middleware
{
    public interface IBusMiddleware<in T> where T: IBusExecutionContext
    {
        public Task Invoke(BusMiddlewareExecutionDelegate next, T context);
    }
}