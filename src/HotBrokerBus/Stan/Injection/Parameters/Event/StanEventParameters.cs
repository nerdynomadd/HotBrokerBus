using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Middleware;

namespace HotBrokerBus.Stan.Extensions.Parameters.Event
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