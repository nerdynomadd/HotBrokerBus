using System;
using Autofac;
using Bus.Abstractions.Events;
using Bus.Middleware;
using STAN.Client;

namespace Bus.Stan.Events
{
    public class StanBusEventExecutionContext : IEventExecutionContext
    {
        public StanBusEventExecutionContext(BusMiddlewareComponent middlewareComponent,
            byte[] eventData,
            Type eventType,
            IEvent @event,
            Type eventHandlerType,
            IEventHandler eventHandler,
            StanMsgHandlerArgs busArguments,
            ILifetimeScope lifetimeScope)
        {
            MiddlewareComponent = middlewareComponent;
            
            EventData = eventData;

            EventType = eventType;
            
            Event = @event;

            EventHandlerType = eventHandlerType;
            
            EventHandler = eventHandler;
            
            BusArguments = busArguments;

            LifetimeScope = lifetimeScope;
        }

        public BusMiddlewareComponent MiddlewareComponent { get; }

        public byte[] EventData { get; }
        
        public Type EventType { get; }

        public IEvent Event { get; set; }
        
        public Type EventHandlerType { get; }

        public IEventHandler EventHandler { get; set; }

        public StanMsgHandlerArgs BusArguments { get; }
        
        public ILifetimeScope LifetimeScope { get; }
    }
}