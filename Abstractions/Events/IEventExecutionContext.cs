using System;
using Autofac;
using STAN.Client;

namespace Bus.Abstractions.Events
{
    public interface IEventExecutionContext : IBusExecutionContext
    {
        public byte[] EventData { get; }
        
        public Type EventType { get; }

        public IEvent Event { get; set; }
        
        public Type EventHandlerType { get; }
        
        public IEventHandler EventHandler { get; set; }

        public StanMsgHandlerArgs BusArguments { get; }
        
        public ILifetimeScope LifetimeScope { get; }

    }
}