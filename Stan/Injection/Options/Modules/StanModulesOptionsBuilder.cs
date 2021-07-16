using System;
using HotBrokerBus.Stan.Extensions.Configuration.Modules;
using HotBrokerBus.Stan.Extensions.Options.Modules.Connection;
using Microsoft.Extensions.Configuration;

namespace HotBrokerBus.Stan.Extensions.Options.Modules
{
    public class StanModulesOptionsBuilder
    {
        private StanModulesParameters _stanModulesParameters;

        private StanModulesConnectionOptionsBuilder _stanModulesConnectionOptionsBuilder;
        
        public StanModulesOptionsBuilder()
        {
            _stanModulesParameters = new();

            _stanModulesConnectionOptionsBuilder = new(_stanModulesParameters.Connection);
        }

        public StanModulesOptionsBuilder ConfigureConnection(Action<StanModulesConnectionOptionsBuilder> options)
        {
            options(_stanModulesConnectionOptionsBuilder);
            
            return this;
        }

        public StanModulesOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanModulesParameters);

            return this;
        }
        
        internal StanModulesOptions Build()
        {
            StanModulesConnectionOptions connectionOptions = _stanModulesConnectionOptionsBuilder.Build();

            return new StanModulesOptions(connectionOptions);
        }
    }
}