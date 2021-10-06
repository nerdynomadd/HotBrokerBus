using HotBrokerBus.Abstractions.Middleware.Events;

namespace HotBrokerBus.Abstractions.Stan.Events.Middleware
{
	public interface IStanEventBusMiddleware : IEventBusMiddleware<IStanEventBusExecutionContext>
	{
		
	}
}