using HotBrokerBus.Stan.Injection.Options.Command.Middleware.Config;

namespace HotBrokerBus.Stan.Injection.Options.Command.Middleware
{
    public class StanCommandBusMiddlewareOptions
    {
        public StanCommandBusMiddlewareOptions(StanCommandBusMiddlewareConfigOptions config)
        {
            Config = config;
        }
        
        internal StanCommandBusMiddlewareConfigOptions Config { get; set; }
    }
}