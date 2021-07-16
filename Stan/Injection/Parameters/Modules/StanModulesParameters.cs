using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Extensions.Configuration.Modules.Connection;

namespace HotBrokerBus.Stan.Extensions.Configuration.Modules
{
    public class StanModulesParameters
    {
        public StanModulesParameters()
        {
            Connection = new StanModulesConnectionParameters();
        }
        
        [JsonPropertyName("connection")]
        public StanModulesConnectionParameters Connection { get; set; }
    }
}