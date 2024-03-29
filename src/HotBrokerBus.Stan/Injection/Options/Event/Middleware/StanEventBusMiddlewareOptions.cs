﻿using HotBrokerBus.Stan.Injection.Options.Event.Middleware.Config;

namespace HotBrokerBus.Stan.Injection.Options.Event.Middleware
{
    public class StanEventBusMiddlewareOptions
    {
        public StanEventBusMiddlewareOptions(StanEventBusMiddlewareConfigOptions config)
        {
            Config = config;
        }
        
        internal StanEventBusMiddlewareConfigOptions Config { get; set; }
    }
}