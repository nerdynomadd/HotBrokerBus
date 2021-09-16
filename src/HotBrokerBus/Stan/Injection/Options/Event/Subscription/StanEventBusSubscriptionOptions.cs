using HotBrokerBus.Stan.Extensions.Options.Event.Event.Config;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Event
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