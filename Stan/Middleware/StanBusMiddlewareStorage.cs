using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Abstractions.Stan.Middleware;
using HotBrokerBus.Middleware;
using HotBrokerBus.Abstractions;
using HotBrokerBus.Abstractions.Middleware;
using Quartz;

namespace HotBrokerBus.Stan.Middleware
{
    public class StanBusMiddlewareStorage : IStanBusMiddlewareStorage
    {
        private enum ComputeBuildType
        {
            Event,
            Command
        }
        
        private readonly ILifetimeScope _lifetimeScope;
        
        private LinkedList<BusMiddlewareComponent> _commandMiddlewareComponents;

        private LinkedList<BusMiddlewareComponent> _eventMiddlewareComponents;

        public StanBusMiddlewareStorage(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            
            _commandMiddlewareComponents = new LinkedList<BusMiddlewareComponent>();
            
            _eventMiddlewareComponents = new LinkedList<BusMiddlewareComponent>();
        }

        public void AddEventMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T : IEventBusMiddleware
        {
            AddEventMiddleware<T>(typeof(T).FullName, priority);
        }

        public void AddEventMiddleware<T>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T : IEventBusMiddleware
        {
            if (HasMiddleware(name))
            {
                throw new ObjectAlreadyExistsException("A middleware with the same name is already registered");
            }

            _eventMiddlewareComponents.AddFirst(new BusMiddlewareComponent(name, priority, typeof(T)));

            _computeEventsBuild();
        }

        public void AddCommandMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T : ICommandBusMiddleware
        {
            AddCommandMiddleware<T>(typeof(T).FullName, priority);
        }

        public void AddCommandMiddleware<T>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T : ICommandBusMiddleware
        {
            if (HasMiddleware(name))
            {
                throw new ObjectAlreadyExistsException("A middleware with the same name is already registered");
            }

            _commandMiddlewareComponents.AddFirst(new BusMiddlewareComponent(name, priority, typeof(T)));

            _computeCommandsBuild();
        }

        private void _computeEventsBuild()
        {
            _eventMiddlewareComponents = computeBuild(_eventMiddlewareComponents, ComputeBuildType.Event);
        }

        private void _computeCommandsBuild()
        {
            _commandMiddlewareComponents = computeBuild(_commandMiddlewareComponents, ComputeBuildType.Command);
        }

        private LinkedList<BusMiddlewareComponent> computeBuild(LinkedList<BusMiddlewareComponent> computeMiddlewareComponents, ComputeBuildType computeBuildType)
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
                    middleware = _lifetimeScope.Resolve(middlewareComponent.Value.Component);
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
                        nextMiddleware = _lifetimeScope.Resolve(middlewareComponent.Next.Value.Component);
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

        public bool HasMiddleware(string name)
        {
            return _commandMiddlewareComponents.Any(e => e.Name == name) || _eventMiddlewareComponents.Any(e => e.Name == name);
        }

        public LinkedList<BusMiddlewareComponent> GetEventMiddlewares()
        {
            return _eventMiddlewareComponents;
        }
        
        public LinkedList<BusMiddlewareComponent> GetCommandMiddlewares()
        {
            return _commandMiddlewareComponents;
        }
    }
}