using System;
using Autofac;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Middleware;
using STAN.Client;

namespace HotBrokerBus.Stan.Events
{
    public class StanBusEventExecutionContext : IEventExecutionContext
    {
        public StanBusEventExecutionContext(BusMiddlewareComponent middlewareComponent,
            string eventTopic,
            byte[] eventData,
            Type eventType,
            IEvent @event,
            Type eventHandlerType,
            IEventHandler eventHandler,
            StanMsgHandlerArgs busArguments,
            ILifetimeScope lifetimeScope)
        {
            MiddlewareComponent = middlewareComponent;

            EventTopic = eventTopic;
            
            EventData = eventData;

            EventType = eventType;
            
            Event = @event;

            EventHandlerType = eventHandlerType;
            
            EventHandler = eventHandler;
            
            BusArguments = busArguments;

            LifetimeScope = lifetimeScope;
        }

        public BusMiddlewareComponent MiddlewareComponent { get; }

        public string EventTopic { get; }
        
        public byte[] EventData { get; }
        
        public Type EventType { get; }

        public IEvent Event { get; set; }
        
        public Type EventHandlerType { get; }

        public IEventHandler EventHandler { get; set; }

        public StanMsgHandlerArgs BusArguments { get; }
        
        public ILifetimeScope LifetimeScope { get; }
    }
}