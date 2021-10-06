using HotBrokerBus.Abstractions.Middleware;

namespace HotBrokerBus.Abstractions.Stan.Middleware
{
	public interface IStanBusMiddleware<in TMiddleware> : IBusMiddleware<IBusExecutionContext>
	{
		
	}
}