using System;
using HotBrokerBus.Stan.Injection.Options.Modules.Connection.StanOptions;
using HotBrokerBus.Stan.Injection.Parameters.Modules.Connection;
using Microsoft.Extensions.Configuration;

namespace HotBrokerBus.Stan.Injection.Options.Modules.Connection
{
    public class StanModulesConnectionOptionsBuilder
    {
        private StanModulesConnectionParameters _stanModulesConnectionParameters;

        private StanModulesConnectionStanOptionsOptionsBuilder _stanOptionsOptionsBuilder;
        
        public StanModulesConnectionOptionsBuilder(StanModulesConnectionParameters stanModulesConnectionParameters)
        {
            _stanModulesConnectionParameters = stanModulesConnectionParameters;

            _stanOptionsOptionsBuilder = new(_stanModulesConnectionParameters.StanOptions);
        }

        public StanModulesConnectionOptionsBuilder ConfigureStanOptions(Action<StanModulesConnectionStanOptionsOptionsBuilder> options)
        {
            options(_stanOptionsOptionsBuilder);
            
            return this;
        }
        
        public StanModulesConnectionOptionsBuilder WithClusterName(string clusterName)
        {
            _stanModulesConnectionParameters.ClusterName = clusterName;

            return this;
        }
        
        public StanModulesConnectionOptionsBuilder WithAppName(string appName)
        {
            _stanModulesConnectionParameters.AppName = appName;

            return this;
        }

        public StanModulesConnectionOptionsBuilder WithParameters(StanModulesConnectionParameters parameters)
        {
            _stanModulesConnectionParameters = parameters;

            return this;
        }

        public StanModulesConnectionOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanModulesConnectionParameters);

            return this;
        }

        internal StanModulesConnectionOptions Build()
        {

            var clusterName = _stanModulesConnectionParameters.ClusterName;

            var appName = $"{_stanModulesConnectionParameters.AppName}_{Guid.NewGuid()}";

            var stanOptionsOptions = _stanOptionsOptionsBuilder.Build();
            
            return new StanModulesConnectionOptions(clusterName, appName, stanOptionsOptions);

        }
    }
}