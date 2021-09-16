using System.Text.Json.Serialization;
using HotBrokerBus.Stan.Extensions.Configuration.Modules.Connection;
using HotBrokerBus.Stan.Extensions.Configuration.Modules.HostedService;

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
        
        [JsonPropertyName("hostedService")]
        public StanModulesHostedServiceParameters HostedService { get; set; }
    }
}