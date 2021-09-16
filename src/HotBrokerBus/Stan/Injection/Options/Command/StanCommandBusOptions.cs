using System.Collections.Generic;
using HotBrokerBus.Stan.Extensions.Options.Event.Event;
using HotBrokerBus.Stan.Extensions.Options.Event.Middleware;

namespace HotBrokerBus.Stan.Extensions.Options.Event
{
    public class StanCommandBusOptions
    {
        public StanCommandBusOptions(StanCommandBusMiddlewareOptions middlewares,
            StanCommandBusSubscriptionOptions subscriptions)
        {
            Middlewares = middlewares;

            Subscriptions = subscriptions;
        }
        
        internal StanCommandBusMiddlewareOptions Middlewares;

        internal StanCommandBusSubscriptionOptions Subscriptions;
    }
}