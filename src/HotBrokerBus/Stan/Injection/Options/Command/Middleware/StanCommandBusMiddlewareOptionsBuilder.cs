using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Middleware;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Middleware;
using Microsoft.Extensions.Configuration;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Middleware
{
    public class StanCommandBusMiddlewareOptionsBuilder
    {
        private List<StanEventMiddlewareParameters> _stanEventMiddlewareParameters;
        
        private readonly StanCommandBusMiddlewareConfigOptionsBuilder _stanCommandBusMiddlewareConfigOptionsBuilder;
        
        public StanCommandBusMiddlewareOptionsBuilder(List<StanEventMiddlewareParameters> stanEventMiddlewareParameters)
        {
            _stanEventMiddlewareParameters = stanEventMiddlewareParameters;
            
            _stanCommandBusMiddlewareConfigOptionsBuilder = new(_stanEventMiddlewareParameters);
        }

        public StanCommandBusMiddlewareOptionsBuilder AddMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic)
            where T: ICommandBusMiddleware
        {
            _stanEventMiddlewareParameters.Add(new StanEventMiddlewareParameters
            {
                Type = typeof(T).Name,
                
                Priority = priority
            });

            _stanCommandBusMiddlewareConfigOptionsBuilder.Bind<T>();

            return this;
        }

        public StanCommandBusMiddlewareOptionsBuilder Bind<T>()
            where T: ICommandBusMiddleware
        {
            _stanCommandBusMiddlewareConfigOptionsBuilder.Bind<T>();

            return this;
        }
        
        public StanCommandBusMiddlewareOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanEventMiddlewareParameters);
            
            return this;
        }

        internal StanCommandBusMiddlewareOptions Build()
        {

            var configOptions = _stanCommandBusMiddlewareConfigOptionsBuilder.Build();

            return new StanCommandBusMiddlewareOptions(configOptions);

        }
    }
}