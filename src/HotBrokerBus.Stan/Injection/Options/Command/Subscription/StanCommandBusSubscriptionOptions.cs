using HotBrokerBus.Stan.Injection.Options.Command.Subscription.Config;

namespace HotBrokerBus.Stan.Injection.Options.Command.Subscription
{
    public class StanCommandBusSubscriptionOptions
    {
        public StanCommandBusSubscriptionOptions(StanCommandBusSubscriptionConfigOptions config)
        {
            Config = config;
        }
        
        internal StanCommandBusSubscriptionConfigOptions Config { get; set; }
    }
}