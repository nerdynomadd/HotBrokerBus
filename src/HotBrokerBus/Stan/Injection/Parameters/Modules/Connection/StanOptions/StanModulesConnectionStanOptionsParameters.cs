using System.Text.Json.Serialization;

namespace HotBrokerBus.Stan.Extensions.Configuration.Modules.Connection.StanOptions
{
    public class StanModulesConnectionStanOptionsParameters
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}