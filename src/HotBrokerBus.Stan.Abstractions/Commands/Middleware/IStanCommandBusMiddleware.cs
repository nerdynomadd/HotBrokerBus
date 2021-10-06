using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Stan.Middleware;

namespace HotBrokerBus.Abstractions.Stan.Commands.Middleware
{
	public interface IStanCommandBusMiddleware : ICommandBusMiddleware<IStanCommandBusExecutionContext>
	{
		
	}
}