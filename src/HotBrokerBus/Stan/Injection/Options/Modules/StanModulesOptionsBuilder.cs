using System;
using HotBrokerBus.Stan.Extensions.Configuration.Modules;
using HotBrokerBus.Stan.Extensions.Options.Modules.Connection;
using HotBrokerBus.Stan.Extensions.Options.Modules.HostedService;
using Microsoft.Extensions.Configuration;

namespace HotBrokerBus.Stan.Extensions.Options.Modules
{
    public class StanModulesOptionsBuilder
    {
        private StanModulesParameters _stanModulesParameters;

        private StanModulesConnectionOptionsBuilder _stanModulesConnectionOptionsBuilder;

        private StanModulesHostedServiceOptionsBuilder _stanModulesHostedServiceOptionsBuilder;
        
        public StanModulesOptionsBuilder()
        {
            _stanModulesParameters = new();

            _stanModulesConnectionOptionsBuilder = new(_stanModulesParameters.Connection);

            _stanModulesHostedServiceOptionsBuilder = new(_stanModulesParameters.HostedService);
        }

        public StanModulesOptionsBuilder ConfigureConnection(Action<StanModulesConnectionOptionsBuilder> options)
        {
            options(_stanModulesConnectionOptionsBuilder);
            
            return this;
        }

        public StanModulesOptionsBuilder ConfigureHostedService(Action<StanModulesHostedServiceOptionsBuilder> options)
        {
            options(_stanModulesHostedServiceOptionsBuilder);

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

            StanModulesHostedServiceOptions hostedServiceOptions = _stanModulesHostedServiceOptionsBuilder.Build();
            
            return new StanModulesOptions(connectionOptions, hostedServiceOptions);
        }
    }
}