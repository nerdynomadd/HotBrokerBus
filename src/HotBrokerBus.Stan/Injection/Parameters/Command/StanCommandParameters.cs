using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Injection.Parameters.Command.Command;
using HotBrokerBus.Stan.Injection.Parameters.Event.Middleware;

namespace HotBrokerBus.Stan.Injection.Parameters.Command
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