using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Injection.Parameters.Modules.Connection;
using HotBrokerBus.Stan.Injection.Parameters.Modules.HostedService;

namespace HotBrokerBus.Stan.Injection.Parameters.Modules
{
    public class StanModulesParameters
    {
        public StanModulesParameters()
        {
            Connection = new StanModulesConnectionParameters();

            HostedService = new StanModulesHostedServiceParameters();
        }
        
        [JsonPropertyName("connection")]
        public StanModulesConnectionParameters Connection { get; set; }
        
        [JsonPropertyName("hostedService")]
        public StanModulesHostedServiceParameters HostedService { get; set; }
    }
}