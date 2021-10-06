using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Injection.Parameters.Command.Command.Subscription;
using HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription.SubscriptionOptions;

namespace HotBrokerBus.Stan.Injection.Parameters.Command.Command
{
    public class StanCommandCommandParameters
    {

        public StanCommandCommandParameters()
        {
            Config = new StanEventEventSubscriptionOptionsParameters();

            Subscriptions = new List<StanCommandCommandSubscriptionParameters>();
        }
        
        [JsonPropertyName("config")]
        public StanEventEventSubscriptionOptionsParameters Config { get; set; }
        
        [JsonPropertyName("subscriptions")]
        public List<StanCommandCommandSubscriptionParameters> Subscriptions { get; set; }
        
    }
}