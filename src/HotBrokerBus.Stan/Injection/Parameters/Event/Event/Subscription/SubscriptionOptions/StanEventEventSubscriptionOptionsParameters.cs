using System.Text.Json.Serialization;

namespace HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription.SubscriptionOptions
{
    public class StanEventEventSubscriptionOptionsParameters
    {
        [JsonPropertyName("autoAck")]
        public bool? AutoAck { get; set; }
        
        [JsonPropertyName("ackTimeout")]
        public int? AckTimeout { get; set; }
        
        [JsonPropertyName("durableName")]
        public string DurableName { get; set; }
        
        [JsonPropertyName("maxInFlight")]
        public int? MaxInFlight { get; set; }
        
        [JsonPropertyName("leaveOpen")]
        public bool? LeaveOpen { get; set; }
        
        [JsonPropertyName("behavior")]
        public StanEventEventSubscriptionOptionsBehavior Behavior { get; set; }

        [JsonPropertyName("behaviorAdditional")]
        public string BehaviorAdditional { get; set; }
    }

    public enum StanEventEventSubscriptionOptionsBehavior
    {
        DeliverAllAvailable,
        StartWithLastReceived,
        StartAt,
    }
}