namespace HotBrokerBus.Stan.Extensions.Options.Modules.HostedService
{
	public class StanModulesHostedServiceOptions
	{
		public StanModulesHostedServiceOptions(bool active,
			bool throwsStartingException)
		{
			Active = active;

			ThrowsStartingException = throwsStartingException;
		}
		
		internal bool Active { get; set; }
		
		internal bool ThrowsStartingException { get; set; }
	}
}