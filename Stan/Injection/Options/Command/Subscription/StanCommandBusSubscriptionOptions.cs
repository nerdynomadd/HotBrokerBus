using HotBrokerBus.Stan.Extensions.Options.Event.Event.Config;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Event
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