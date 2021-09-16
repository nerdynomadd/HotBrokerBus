using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Middleware;

namespace HotBrokerBus.Stan.Extensions.Parameters.Event
{
    public class StanCommandParameters
    {
        public StanCommandParameters()
        {
            Middlewares = new List<StanEventMiddlewareParameters>();

            Commands = new StanCommandCommandParameters();
        }
        
        [JsonPropertyName("middlewares")]
        public List<StanEventMiddlewareParameters> Middlewares { get; set; }
        
        [JsonPropertyName("commands")]
        public StanCommandCommandParameters Commands { get; set; }
    }
}