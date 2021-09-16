using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event.StanSubscriptionOptions;

namespace HotBrokerBus.Stan.Extensions.Parameters.Event.Event
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