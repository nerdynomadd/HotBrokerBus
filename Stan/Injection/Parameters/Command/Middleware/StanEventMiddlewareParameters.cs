using System.Text.Json.Serialization;
using HotBrokerBus.Middleware;

namespace HotBrokerBus.Stan.Extensions.Parameters.Event.Middleware
{
    public class StanCommandMiddlewareParameters
    {
        [JsonPropertyName("priority")]
        public BusMiddlewarePriority Priority { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}