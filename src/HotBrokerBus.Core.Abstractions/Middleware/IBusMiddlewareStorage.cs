using System.Collections.Generic;

namespace HotBrokerBus.Abstractions.Middleware
{
    public interface IBusMiddlewareStorage<in TMiddlewareType>
    {
        public void AddMiddleware<TMiddleware>(BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) 
            where TMiddleware: TMiddlewareType;
        
        public void AddMiddleware<TMiddleware>(string name, BusMiddlewarePriority priority = BusMiddlewarePriority.Basic) 
            where TMiddleware: TMiddlewareType;

        public bool HasMiddleware(string name);

        public LinkedList<IBusMiddlewareComponent> GetMiddlewares();
    }
}