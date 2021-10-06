using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Stan.Injection.Parameters.Command.Command.Subscription;
using Microsoft.Extensions.DependencyInjection;

namespace HotBrokerBus.Stan.Injection.Options.Command.Subscription.Config
{
    public class StanCommandBusSubscriptionConfigOptionsBuilder
    {

        private readonly List<StanCommandCommandSubscriptionParameters> _stanCommandCommandParameters;

        private readonly Dictionary<string, Tuple<Type, Type>> _typesToBind;

        private readonly IServiceCollection _serviceCollection;
        
        public StanCommandBusSubscriptionConfigOptionsBuilder(List<StanCommandCommandSubscriptionParameters> stanCommandCommandParameters, IServiceCollection serviceCollection)
        {
            _stanCommandCommandParameters = stanCommandCommandParameters;

            _typesToBind = new();

            _serviceCollection = serviceCollection;
        }

        public StanCommandBusSubscriptionConfigOptionsBuilder Bind<T, T2>()
            where T: ICommand
            where T2 : ICommandHandler
        {
            var typeName = typeof(T).Name;

            if (!_typesToBind.ContainsKey(typeName))
            {
                _typesToBind.Add(typeName, new Tuple<Type, Type>(typeof(T), typeof(T2)));

                _serviceCollection.AddTransient(typeof(T2));
            }

            return this;
        }

        internal StanCommandBusSubscriptionConfigOptions Build()
        {
            Dictionary<string, Tuple<Type, Type>> eventsMap = new();
            
            foreach (var command in _stanCommandCommandParameters)
            {
                if (!_typesToBind.ContainsKey(command.Type))
                {
                    throw new ArgumentException(
                        $"The type {command.Type} is not bound. Add options.Bind<${command.Type}>() after stanEventOptions.WithConfig().");
                }
                
                var types = _typesToBind[command.Type];
                
                eventsMap.Add(command.SubscriptionName, new Tuple<Type, Type>(types.Item1, types.Item2));
            }

            return new StanCommandBusSubscriptionConfigOptions(eventsMap);
        }

    }
}