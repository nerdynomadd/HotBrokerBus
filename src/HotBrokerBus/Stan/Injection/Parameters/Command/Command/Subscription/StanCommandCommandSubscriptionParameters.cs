using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event.StanSubscriptionOptions;

namespace HotBrokerBus.Stan.Extensions.Parameters.Event.Event
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