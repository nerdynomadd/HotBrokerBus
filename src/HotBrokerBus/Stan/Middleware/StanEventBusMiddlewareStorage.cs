using System;
using System.Collections.Generic;
using System.Linq;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Abstractions.Stan.Middleware;
using HotBrokerBus.Middleware;
using HotBrokerBus.Stan.Extensions.Options.Event;
using Quartz;

namespace HotBrokerBus.Stan.Middleware
{
    public sealed class StanEventBusMiddlewareStorage : StanBusMiddlewareStorage<IEventBusMiddleware>, IStanEventBusMiddlewareStorage
    {
        public StanEventBusMiddlewareStorage(IServiceProvider serviceProvider, StanEventBusOptions stanEventBusOptions) : base(serviceProvider)
        {
            _buildFromOptions(stanEventBusOptions);
        }

        private void _buildFromOptions(StanEventBusOptions stanEventBusOptions)
        {
            foreach ((var middlewareType, var middlewarePriority) in stanEventBusOptions.Middlewares.Config.Middlewares)
            {

                typeof(StanEventBusMiddlewareStorage)
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

            MiddlewareComponents = ComputeBuild(MiddlewareComponents, ComputeBuildType.Event);
        }
    }
}