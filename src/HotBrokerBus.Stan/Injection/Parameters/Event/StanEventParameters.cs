using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Injection.Parameters.Event.Event;
using HotBrokerBus.Stan.Injection.Parameters.Event.Middleware;

namespace HotBrokerBus.Stan.Injection.Parameters.Event
{
    public class StanEventParameters
    {
        public StanEventParameters()
        {
            Middlewares = new List<StanEventMiddlewareParameters>();

            Events = new StanEventEventParameters();
        }
        
        [JsonPropertyName("middlewares")]
        public List<StanEventMiddlewareParameters> Middlewares { get; set; }
        
        [JsonPropertyName("events")]
        public StanEventEventParameters Events { get; set; }
    }
}