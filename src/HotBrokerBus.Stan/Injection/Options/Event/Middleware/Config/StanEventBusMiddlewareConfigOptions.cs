using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware;

namespace HotBrokerBus.Stan.Injection.Options.Event.Middleware.Config
{
    public class StanEventBusMiddlewareConfigOptions
    {
        public StanEventBusMiddlewareConfigOptions(Dictionary<Type, BusMiddlewarePriority> middlewares)
        {
            Middlewares = middlewares;
        }
        
        internal Dictionary<Type, BusMiddlewarePriority> Middlewares { get; }
    }
}