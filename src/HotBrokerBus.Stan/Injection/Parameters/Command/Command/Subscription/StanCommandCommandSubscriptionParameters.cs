using System.Text.Json.Serialization;

namespace HotBrokerBus.Stan.Injection.Parameters.Command.Command.Subscription
{
    public class StanCommandCommandSubscriptionParameters
    {
        
        [JsonPropertyName("subscriptionName")]
        public string SubscriptionName { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("handlerType")]
        public string HandlerType { get; set; }

    }
}