using System.Text.Json.Serialization;
using HotBrokerBus.Abstractions.Middleware;

namespace HotBrokerBus.Stan.Injection.Parameters.Event.Middleware
{
    public class StanEventMiddlewareParameters
    {
        [JsonPropertyName("priority")]
        public BusMiddlewarePriority Priority { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}