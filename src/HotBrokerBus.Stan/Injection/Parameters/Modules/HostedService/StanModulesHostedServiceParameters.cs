using System.Text.Json.Serialization;

namespace HotBrokerBus.Stan.Injection.Parameters.Modules.HostedService
{
	public class StanModulesHostedServiceParameters
	{

		[JsonPropertyName("active")]
		public bool Active { get; set; } = true;

		[JsonPropertyName("throwsStartingException")]
		public bool ThrowsStartingException { get; set; } = true;

	}
}