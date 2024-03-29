﻿using System;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Middleware;
using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Commands.Middleware;
using HotBrokerBus.Abstractions.Stan.Middleware;
using HotBrokerBus.Stan.Injection.Options.Command;
using Quartz;

namespace HotBrokerBus.Stan.Middleware
{
    public class StanCommandBusMiddlewareStorage : StanBusMiddlewareStorage<ICommandBusMiddleware<IStanCommandBusExecutionContext>>, IStanCommandBusMiddlewareStorage
    {
        public StanCommandBusMiddlewareStorage(IServiceProvider serviceProvider, StanCommandBusOptions stanCommandBusOptions) : base(serviceProvider)
        {
            _buildFromOptions(stanCommandBusOptions);
        }
        
        private void _buildFromOptions(StanCommandBusOptions stanCommandBusOptions)
        {
            foreach ((var middlewareType, var middlewarePriority) in stanCommandBusOptions.Middlewares.Config.Middlewares)
            {

                typeof(StanCommandBusMiddlewareStorage)
                    .GetMethod(nameof(AddMiddleware), new Type[]{ typeof(BusMiddlewarePriority) })
                    .MakeGenericMethod(middlewareType)
                    .Invoke(this, new object?[] { middlewarePriority });

            }
        }

        public override void AddMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic)
        {
            AddMiddleware<T>(typeof(T).FullName, priority);
        }

        public override void AddMiddleware<T>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic)
        {
            if (HasMiddleware(name))
            {
                throw new ObjectAlreadyExistsException("A middleware with the same name is already registered");
            }

            MiddlewareComponents.AddFirst(new BusMiddlewareComponent(name, priority, typeof(T)));
            
            MiddlewareComponents = ComputeBuild(MiddlewareComponents, ComputeBuildType.Command);
        }
    }
}