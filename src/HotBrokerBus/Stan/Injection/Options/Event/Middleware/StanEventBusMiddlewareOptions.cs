namespace HotBrokerBus.Stan.Extensions.Options.Event.Middleware
{
    public class StanEventBusMiddlewareOptions
    {
        public StanEventBusMiddlewareOptions(StanEventBusMiddlewareConfigOptions config)
        {
            Config = config;
        }
        
        internal StanEventBusMiddlewareConfigOptions Config { get; set; }
    }
}