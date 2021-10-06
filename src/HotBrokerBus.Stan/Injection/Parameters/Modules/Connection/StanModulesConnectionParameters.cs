using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Injection.Parameters.Modules.Connection.StanOptions;

namespace HotBrokerBus.Stan.Injection.Parameters.Modules.Connection
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