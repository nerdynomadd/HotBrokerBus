using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription;
using HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription.SubscriptionOptions;

namespace HotBrokerBus.Stan.Injection.Parameters.Event.Event
{
    public class StanEventEventParameters
    {

        public StanEventEventParameters()
        {
            Config = new StanEventEventSubscriptionOptionsParameters();
            
            Subscriptions = new List<StanEventEventSubscriptionParameters>();
        }
        
        [JsonPropertyName("config")]
        public StanEventEventSubscriptionOptionsParameters Config { get; set; }
        
        [JsonPropertyName("subscriptions")]
        public List<StanEventEventSubscriptionParameters> Subscriptions { get; set; }
        
    }
}