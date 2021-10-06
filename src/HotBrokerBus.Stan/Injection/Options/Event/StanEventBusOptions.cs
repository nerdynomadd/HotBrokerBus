using HotBrokerBus.Stan.Injection.Options.Event.Middleware;
using HotBrokerBus.Stan.Injection.Options.Event.Subscription;

namespace HotBrokerBus.Stan.Injection.Options.Event
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