using System.Text.Json.Serialization;
using HotBrokerBus.Abstractions.Middleware;

namespace HotBrokerBus.Stan.Injection.Parameters.Command.Middleware
{
    public class StanCommandMiddlewareParameters
    {
        [JsonPropertyName("priority")]
        public BusMiddlewarePriority Priority { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}