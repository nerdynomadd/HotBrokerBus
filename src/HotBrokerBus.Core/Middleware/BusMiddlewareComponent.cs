using System;

namespace HotBrokerBus.Abstractions.Middleware
{
    public class BusMiddlewareComponent : IBusMiddlewareComponent
    {
        public BusMiddlewareComponent(string name,
            BusMiddlewarePriority priority,
            Type component)
        {
            Name = name;

            Priority = priority;
            
            Component = component;
        }

        public string Name { get; }

        public BusMiddlewarePriority Priority { get; }

        public Type Component { get; }

        public BusMiddlewareExecutionDelegate Next { get; set; }

        public BusMiddlewareExecutionDelegate Process { get; set; }
    }
}