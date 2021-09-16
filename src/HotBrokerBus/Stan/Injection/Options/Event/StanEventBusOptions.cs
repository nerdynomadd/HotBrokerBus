using System.Collections.Generic;
using HotBrokerBus.Stan.Extensions.Options.Event.Event;
using HotBrokerBus.Stan.Extensions.Options.Event.Middleware;

namespace HotBrokerBus.Stan.Extensions.Options.Event
{
    public class StanEventBusOptions
    {
        public StanEventBusOptions(StanEventBusMiddlewareOptions middlewares,
            StanEventBusSubscriptionOptions subscriptions)
        {
            Middlewares = middlewares;

            Subscriptions = subscriptions;
        }
        
        internal StanEventBusMiddlewareOptions Middlewares;

        internal StanEventBusSubscriptionOptions Subscriptions;
    }
}