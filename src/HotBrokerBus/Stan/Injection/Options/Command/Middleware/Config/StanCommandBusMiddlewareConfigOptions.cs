using System;
using System.Collections.Generic;
using HotBrokerBus.Middleware;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Middleware
{
    public class StanCommandBusMiddlewareConfigOptions
    {
        public StanCommandBusMiddlewareConfigOptions(Dictionary<Type, BusMiddlewarePriority> middlewares)
        {
            Middlewares = middlewares;
        }
        
        internal Dictionary<Type, BusMiddlewarePriority> Middlewares { get; }
    }
}