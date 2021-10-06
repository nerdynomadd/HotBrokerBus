using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription.SubscriptionOptions;

namespace HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription
{
    public class StanEventEventSubscriptionParameters
    {
        
        [JsonPropertyName("subscriptionName")]
        public string SubscriptionName { get; set; }
        
        [JsonPropertyName("queueGroup")]
        public string QueueGroup { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("handlerType")]
        public string HandlerType { get; set; }
        
        [JsonPropertyName("subscriptionOptions")]
        public StanEventEventSubscriptionOptionsParameters? SubscriptionOptions { get; set; }
        
    }
}