using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Middleware;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Middleware;
using Microsoft.Extensions.Configuration;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Middleware
{
    public class StanEventBusMiddlewareOptionsBuilder
    {
        private List<StanEventMiddlewareParameters> _stanEventMiddlewareParameters;
        
        private readonly StanEventBusMiddlewareConfigOptionsBuilder _stanEventBusMiddlewareConfigOptionsBuilder;
        
        public StanEventBusMiddlewareOptionsBuilder(List<StanEventMiddlewareParameters> stanEventMiddlewareParameters)
        {
            _stanEventMiddlewareParameters = stanEventMiddlewareParameters;
            
            _stanEventBusMiddlewareConfigOptionsBuilder = new(_stanEventMiddlewareParameters);
        }

        public StanEventBusMiddlewareOptionsBuilder AddMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic)
            where T: IEventBusMiddleware
        {
            _stanEventMiddlewareParameters.Add(new StanEventMiddlewareParameters
            {
                Type = typeof(T).Name,
                
                Priority = priority
            });

            _stanEventBusMiddlewareConfigOptionsBuilder.Bind<T>();

            return this;
        }

        public StanEventBusMiddlewareOptionsBuilder Bind<T>()
            where T: IEventBusMiddleware
        {
            _stanEventBusMiddlewareConfigOptionsBuilder.Bind<T>();

            return this;
        }
        
        public StanEventBusMiddlewareOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanEventMiddlewareParameters);
            
            return this;
        }

        internal StanEventBusMiddlewareOptions Build()
        {

            var configOptions = _stanEventBusMiddlewareConfigOptionsBuilder.Build();

            return new StanEventBusMiddlewareOptions(configOptions);

        }
    }
}