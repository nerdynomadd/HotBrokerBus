using System.Threading.Tasks;
using Bus.Abstractions.Commands;
using Bus.Abstractions.Middleware.Commands;
using Bus.Middleware;

namespace Bus.Abstractions.Stan.Commands
{
    public interface IStanBusCommandExecutionMiddleware : ICommandBusMiddleware
    {
    }
}