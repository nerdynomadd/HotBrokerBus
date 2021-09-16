namespace HotBrokerBus.Stan.Extensions.Options.Event.Middleware
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