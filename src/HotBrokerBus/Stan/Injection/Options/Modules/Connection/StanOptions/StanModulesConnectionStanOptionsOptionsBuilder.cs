using HotBrokerBus.Stan.Extensions.Configuration.Modules.Connection.StanOptions;
using Microsoft.Extensions.Configuration;

namespace HotBrokerBus.Stan.Extensions.Options.Modules.Connection.StanOptions
{
    public class StanModulesConnectionStanOptionsOptionsBuilder
    {
        private StanModulesConnectionStanOptionsParameters _stanModulesConnectionStanOptionsParameters;

        public StanModulesConnectionStanOptionsOptionsBuilder(StanModulesConnectionStanOptionsParameters stanModulesConnectionStanOptionsParameters)
        {
            _stanModulesConnectionStanOptionsParameters = stanModulesConnectionStanOptionsParameters;
        }

        public StanModulesConnectionStanOptionsOptionsBuilder WithUrl(string url)
        {
            _stanModulesConnectionStanOptionsParameters.Url = url;

            return this;
        }

        public StanModulesConnectionStanOptionsOptionsBuilder WithParameters(StanModulesConnectionStanOptionsParameters parameters)
        {
            _stanModulesConnectionStanOptionsParameters = parameters;

            return this;
        }

        public StanModulesConnectionStanOptionsOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanModulesConnectionStanOptionsParameters);

            return this;
        }

        internal StanModulesConnectionStanOptionsOptions Build()
        {
            var stanOptions = STAN.Client.StanOptions.GetDefaultOptions();

            stanOptions.NatsURL = _stanModulesConnectionStanOptionsParameters.Url;
            
            return new StanModulesConnectionStanOptionsOptions(stanOptions);
        }
    }
}