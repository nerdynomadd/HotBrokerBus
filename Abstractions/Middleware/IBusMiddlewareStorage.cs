using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Middleware;

namespace HotBrokerBus.Abstractions.Middleware
{
    public interface IBusMiddlewareStorage
    {
        public void AddEventMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T: IEventBusMiddleware;
        
        public void AddEventMiddleware<T>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T: IEventBusMiddleware;
        
        public void AddCommandMiddleware<T>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T: ICommandBusMiddleware;
        
        public void AddCommandMiddleware<T>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where T: ICommandBusMiddleware;

        public bool HasMiddleware(string name);

        public LinkedList<BusMiddlewareComponent> GetEventMiddlewares();

        public LinkedList<BusMiddlewareComponent> GetCommandMiddlewares();
    }
}