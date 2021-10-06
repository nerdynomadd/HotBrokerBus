using System;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Middleware;
using HotBrokerBus.Abstractions.Stan.Events;
using NATS.Client;
using STAN.Client;

namespace HotBrokerBus.Stan.Events
{
	public class StanEventBusExecutionContext : IStanEventBusExecutionContext
	{
		public StanEventBusExecutionContext(IBusMiddlewareComponent middlewareComponent,
			string eventTopic,
			byte[] eventData,
			Type eventType,
			IEvent @event,
			Type eventHandlerType,
			IEventHandler eventHandler,
			StanMsgHandlerArgs busArguments,
			IServiceProvider serviceProvider)
		{
			MiddlewareComponent = middlewareComponent;

			EventTopic = eventTopic;
            
			EventData = eventData;

			EventType = eventType;
            
			Event = @event;

			EventHandlerType = eventHandlerType;
            
			EventHandler = eventHandler;
            
			BusArguments = busArguments;

			ServiceProvider = serviceProvider;
		}

		public IBusMiddlewareComponent MiddlewareComponent { get; }

		public string EventTopic { get; }
        
		public byte[] EventData { get; }
		
		public Type EventType { get; }

		public IEvent Event { get; set; }
        
		public Type EventHandlerType { get; }

		public IEventHandler EventHandler { get; set; }

		public StanMsgHandlerArgs BusArguments { get; }

		public IServiceProvider ServiceProvider { get; }
	}
}