using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Extensions.Configuration.Modules.Connection.StanOptions;

namespace HotBrokerBus.Stan.Extensions.Configuration.Modules.Connection
{
    public class StanModulesConnectionParameters
    {
        public StanModulesConnectionParameters()
        {
            StanOptions = new StanModulesConnectionStanOptionsParameters();
        }
        
        [JsonPropertyName("clusterName")]
        public string ClusterName { get; set; }
        
        [JsonPropertyName("appName")]
        public string AppName { get; set; }

        [JsonPropertyName("stanOptions")]
        public StanModulesConnectionStanOptionsParameters StanOptions { get; set; }
    }
}