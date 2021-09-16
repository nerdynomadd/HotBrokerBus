using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Middleware;

namespace HotBrokerBus.Abstractions.Stan.Commands
{
    public interface IStanCommandBusExecutionMiddleware : ICommandBusMiddleware
    {
    }
}