using System.Text.Json.Serialization;

namespace HotBrokerBus.Stan.Injection.Parameters.Modules.Connection.StanOptions
{
    public class StanModulesConnectionStanOptionsParameters
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}