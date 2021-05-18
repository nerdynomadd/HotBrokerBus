using System.Threading.Tasks;
using Bus.Middleware;

namespace Bus.Abstractions.Middleware
{
    public interface IBusMiddleware<T> where T: IBusExecutionContext
    {
        public Task Invoke(BusMiddlewareExecutionDelegate next, T context);
    }
}