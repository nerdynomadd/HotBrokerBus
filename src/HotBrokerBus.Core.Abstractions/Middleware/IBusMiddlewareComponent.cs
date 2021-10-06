using System;

namespace HotBrokerBus.Abstractions.Middleware
{
    public interface IBusMiddlewareComponent
    {
        public string Name { get; }

        public BusMiddlewarePriority Priority { get; }

        public Type Component { get; }

        public BusMiddlewareExecutionDelegate Next { get; set; }

        public BusMiddlewareExecutionDelegate Process { get; set; }
    }
}