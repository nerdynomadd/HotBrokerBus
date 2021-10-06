using HotBrokerBus.Stan.Injection.Parameters.Modules.HostedService;

namespace HotBrokerBus.Stan.Injection.Options.Modules.HostedService
{
	public class StanModulesHostedServiceOptionsBuilder
	{
		private readonly StanModulesHostedServiceParameters _parameters;
		
		public StanModulesHostedServiceOptionsBuilder(StanModulesHostedServiceParameters parameters)
		{
			_parameters = parameters;
		}

		public StanModulesHostedServiceOptionsBuilder SetIsActive(bool isActive = true)
		{
			_parameters.Active = isActive;

			return this;
		}
		
		public StanModulesHostedServiceOptionsBuilder SetThrowsStartExceptions(bool throwsStartExceptions = true)
		{
			_parameters.ThrowsStartingException = throwsStartExceptions;
			
			return this;
		}

		internal StanModulesHostedServiceOptions Build()
		{
			return new StanModulesHostedServiceOptions(_parameters.Active, _parameters.ThrowsStartingException);
		}
	}
}