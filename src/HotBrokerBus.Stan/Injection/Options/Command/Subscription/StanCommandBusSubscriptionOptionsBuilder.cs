using System.Collections.Generic;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Stan.Injection.Options.Command.Subscription.Config;
using HotBrokerBus.Stan.Injection.Parameters.Command.Command.Subscription;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotBrokerBus.Stan.Injection.Options.Command.Subscription
{
    public class StanCommandBusSubscriptionOptionsBuilder
    {

        private List<StanCommandCommandSubscriptionParameters> _stanCommandCommandParameters;

        private readonly StanCommandBusSubscriptionConfigOptionsBuilder _stanCommandBusSubscriptionConfigOptionsBuilder;

        public StanCommandBusSubscriptionOptionsBuilder(List<StanCommandCommandSubscriptionParameters> stanCommandCommandParameters, IServiceCollection serviceCollection)
        {

            _stanCommandCommandParameters = stanCommandCommandParameters;

            _stanCommandBusSubscriptionConfigOptionsBuilder = new(_stanCommandCommandParameters, serviceCollection);

        }

        public StanCommandBusSubscriptionOptionsBuilder Subscribe<T, T2>(string subscriptionName)
            where T: ICommand
            where T2 : ICommandHandler
        {
            _stanCommandCommandParameters.Add(new StanCommandCommandSubscriptionParameters
            {
                Type = typeof(T).Name,
                HandlerType = typeof(T2).Name,
                SubscriptionName = subscriptionName
            });

            Bind<T, T2>();

            return this;
        }
        
        public StanCommandBusSubscriptionOptionsBuilder Bind<T, T2>()
            where T: ICommand
            where T2 : ICommandHandler
        {
            _stanCommandBusSubscriptionConfigOptionsBuilder.Bind<T, T2>();

            return this;
        }

        public StanCommandBusSubscriptionOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanCommandCommandParameters);

            return this;
        }

        internal StanCommandBusSubscriptionOptions Build()
        {
            var configOptions = _stanCommandBusSubscriptionConfigOptionsBuilder.Build();

            return new StanCommandBusSubscriptionOptions(configOptions);
        }
        
    }
}