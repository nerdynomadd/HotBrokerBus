using HotBrokerBus.Stan.Injection.Options.Command.Middleware;
using HotBrokerBus.Stan.Injection.Options.Command.Subscription;

namespace HotBrokerBus.Stan.Injection.Options.Command
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