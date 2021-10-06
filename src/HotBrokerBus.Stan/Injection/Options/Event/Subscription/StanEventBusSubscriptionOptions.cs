using HotBrokerBus.Stan.Injection.Options.Event.Subscription.Config;

namespace HotBrokerBus.Stan.Injection.Options.Event.Subscription
{
    public class StanEventBusSubscriptionOptions
    {
        public StanEventBusSubscriptionOptions(StanEventBusSubscriptionConfigOptions config)
        {
            Config = config;
        }
        
        internal StanEventBusSubscriptionConfigOptions Config { get; set; }
    }
}