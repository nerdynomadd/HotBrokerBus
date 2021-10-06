using System;
using HotBrokerBus.Stan.Injection.Options.Modules.Connection;
using HotBrokerBus.Stan.Injection.Options.Modules.HostedService;
using HotBrokerBus.Stan.Injection.Parameters.Modules;
using Microsoft.Extensions.Configuration;

namespace HotBrokerBus.Stan.Injection.Options.Modules
{
    public class StanModulesOptionsBuilder
    {
        private readonly StanModulesParameters _stanModulesParameters;

        private readonly StanModulesConnectionOptionsBuilder _stanModulesConnectionOptionsBuilder;

        private readonly StanModulesHostedServiceOptionsBuilder _stanModulesHostedServiceOptionsBuilder;
        
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
        
        public StanModulesOptions Build()
        {
            StanModulesConnectionOptions connectionOptions = _stanModulesConnectionOptionsBuilder.Build();

            StanModulesHostedServiceOptions hostedServiceOptions = _stanModulesHostedServiceOptionsBuilder.Build();
            
            return new StanModulesOptions(connectionOptions, hostedServiceOptions);
        }
    }
}