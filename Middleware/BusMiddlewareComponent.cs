using System;

namespace Bus.Middleware
{
    public class BusMiddlewareComponent
    {
        public BusMiddlewareComponent(string name,
            BusMiddlewarePriority priority,
            Type component)
        {
            Name = name;

            Priority = priority;
            
            Component = component;
        }

        public readonly string Name;

        public readonly BusMiddlewarePriority Priority;

        public readonly Type Component;

        public BusMiddlewareExecutionDelegate Next;

        public BusMiddlewareExecutionDelegate Process;
    }
}