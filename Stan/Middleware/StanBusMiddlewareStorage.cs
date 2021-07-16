using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Middleware;
using HotBrokerBus.Abstractions.Middleware;

namespace HotBrokerBus.Stan.Middleware
{
    public abstract class StanBusMiddlewareStorage<TMiddlewareType> : IBusMiddlewareStorage<TMiddlewareType>
    {
        protected enum ComputeBuildType
        {
            Event,
            Command
        }
        
        private readonly IServiceProvider _serviceProvider;
        
        protected LinkedList<BusMiddlewareComponent> MiddlewareComponents;

        public StanBusMiddlewareStorage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            MiddlewareComponents = new LinkedList<BusMiddlewareComponent>();
        }

        public abstract void AddMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic)
            where T : TMiddlewareType;

        public abstract void AddMiddleware<T>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic)
            where T : TMiddlewareType;

        public bool HasMiddleware(string name)
        {
            return MiddlewareComponents.Any(e => e.Name == name);
        }
        
        public LinkedList<BusMiddlewareComponent> GetMiddlewares()
        {
            return MiddlewareComponents;
        }        
        protected LinkedList<BusMiddlewareComponent> ComputeBuild(LinkedList<BusMiddlewareComponent> computeMiddlewareComponents, ComputeBuildType computeBuildType)
        {
            var firstMiddlewareComponents = computeMiddlewareComponents.Where(e => e.Priority == BusMiddlewarePriority.First);

            var basicMiddlewareComponents = computeMiddlewareComponents.Where(e => e.Priority == BusMiddlewarePriority.Basic);
            
            var lastMiddlewareComponents = computeMiddlewareComponents.Where(e => e.Priority == BusMiddlewarePriority.Last);

            var middlewareComponents = new LinkedList<BusMiddlewareComponent>();
            
            foreach (var firsMiddlewareComponent in firstMiddlewareComponents)
            {
                middlewareComponents.AddLast(firsMiddlewareComponent);
            }

            foreach (var basicMiddlewareComponent in basicMiddlewareComponents)
            {
                middlewareComponents.AddLast(basicMiddlewareComponent);
            }

            foreach (var lastMiddlewareComponent in lastMiddlewareComponents)
            {
                middlewareComponents.AddLast(lastMiddlewareComponent);
            }

            var middlewareComponent = middlewareComponents.Last;

            while (middlewareComponent != null)
            {
                object middleware;

                if (middlewareComponent.Value.Component.IsInterface)
                {
                    middleware = _serviceProvider.GetService(middlewareComponent.Value.Component);
                }
                else
                {
                    middleware = Activator.CreateInstance(middlewareComponent.Value.Component);
                }

                if (middleware == null)
                {
                    middlewareComponent = middlewareComponent.Previous;
                    
                    continue;
                }
                
                BusMiddlewareExecutionDelegate nextBusMiddlewareDelegate;

                if (middlewareComponent.Next == null)
                {
                    nextBusMiddlewareDelegate = ctx => Task.CompletedTask;
                }
                else
                {
                    object nextMiddleware;

                    if (middlewareComponent.Next.Value.Component.IsInterface)
                    {
                        nextMiddleware = _serviceProvider.GetService(middlewareComponent.Next.Value.Component);
                    }
                    else
                    {
                        nextMiddleware = Activator.CreateInstance(middlewareComponent.Next.Value.Component);
                    }
                    
                    if (nextMiddleware == null)
                    {
                        middlewareComponent = middlewareComponent.Previous;

                        continue;
                    }

                    var nextMiddlewareComponentCopy = middlewareComponent.Next.Value;
                    
                    var nextMiddlewareBaseInterface =
                        nextMiddlewareComponentCopy.Component.GetInterface(
                            "HotBrokerBus.Abstractions.Middleware.IBusMiddleware`1");
                    
                    if (nextMiddlewareBaseInterface == null)
                    {
                        middlewareComponent = middlewareComponent.Previous;

                        continue;
                    }
                    
                    var nextMiddlewareMethod = nextMiddlewareBaseInterface.GetMethod("Invoke");

                    Delegate nextMiddlewareInternalDelegate;
                    
                    if (computeBuildType == ComputeBuildType.Event)
                    {
                        nextMiddlewareInternalDelegate = (InternalEventMiddlewareExecutionDelegate) nextMiddlewareMethod?.CreateDelegate(typeof(InternalEventMiddlewareExecutionDelegate), nextMiddleware);
                    } else
                    {
                        nextMiddlewareInternalDelegate = (InternalCommandMiddlewareExecutionDelegate) nextMiddlewareMethod?.CreateDelegate(typeof(InternalCommandMiddlewareExecutionDelegate), nextMiddleware);
                    }
                    
                    nextBusMiddlewareDelegate = async ctx =>
                    {
                        if (nextMiddlewareInternalDelegate == null)
                        {
                            await Task.CompletedTask;

                            return;
                        }

                        if (computeBuildType == ComputeBuildType.Event)
                        {
                            ctx = ctx as IEventExecutionContext;
                        }
                        else
                        {
                            ctx = ctx as ICommandExecutionContext;
                        }
                    
                        await (Task) nextMiddlewareInternalDelegate.DynamicInvoke(nextMiddlewareComponentCopy.Next, ctx);
                    };
                }
                
                middlewareComponent.Value.Next = nextBusMiddlewareDelegate;
                
                var processMiddlewareComponentCopy = middlewareComponent.Value;

                var processMiddlewareBaseInterface =
                    processMiddlewareComponentCopy.Component.GetInterface(
                        "HotBrokerBus.Abstractions.Middleware.IBusMiddleware`1");

                if (processMiddlewareBaseInterface == null)
                {
                    middlewareComponent = middlewareComponent.Previous;

                    continue;
                }
                
                var processMiddlewareMethod = processMiddlewareBaseInterface.GetMethod("Invoke", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic);

                Delegate processMiddlewareInternalDelegate;

                if (computeBuildType == ComputeBuildType.Event)
                {
                    processMiddlewareInternalDelegate =
                        (InternalEventMiddlewareExecutionDelegate) processMiddlewareMethod?.CreateDelegate(
                            typeof(InternalEventMiddlewareExecutionDelegate), middleware);
                } else
                {
                    processMiddlewareInternalDelegate =
                        (InternalCommandMiddlewareExecutionDelegate) processMiddlewareMethod?.CreateDelegate(
                            typeof(InternalCommandMiddlewareExecutionDelegate), middleware);
                }
                
                middlewareComponent.Value.Process = async ctx =>
                {
                    if (processMiddlewareInternalDelegate == null)
                    {
                        await Task.CompletedTask;

                        return;
                    }
                    
                    if (computeBuildType == ComputeBuildType.Event)
                    {
                        ctx = ctx as IEventExecutionContext;
                    }
                    else
                    {
                        ctx = ctx as ICommandExecutionContext;
                    }
                    
                    await (Task) processMiddlewareInternalDelegate.DynamicInvoke(processMiddlewareComponentCopy.Next, ctx);
                };

                middlewareComponent = middlewareComponent.Previous;
            }

            return middlewareComponents;
        }
    }
}