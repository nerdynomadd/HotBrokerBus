using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Middleware;

namespace HotBrokerBus.Abstractions.Middleware
{
    public interface IBusMiddlewareStorage<TMiddlewareType>
    {
        public void AddMiddleware<TMiddleware>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where TMiddleware: TMiddlewareType;
        
        public void AddMiddleware<TMiddleware>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) where TMiddleware: TMiddlewareType;

        public bool HasMiddleware(string name);

        public LinkedList<BusMiddlewareComponent> GetMiddlewares();
    }
}