using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware;

namespace HotBrokerBus.Stan.Injection.Options.Command.Middleware.Config
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