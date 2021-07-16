using System;
using System.Collections.Generic;
using STAN.Client;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Event.Config
{
    public class StanCommandBusSubscriptionConfigOptions
    {
        public StanCommandBusSubscriptionConfigOptions(Dictionary<string, Tuple<Type, Type>> commands)
        {
            Commands = commands;
        }
        
        internal Dictionary<string, Tuple<Type, Type>> Commands { get; }
    }
}