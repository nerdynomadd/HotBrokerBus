using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription;
using HotBrokerBus.Stan.Injection.Parameters.Event.Event.Subscription.SubscriptionOptions;
using Microsoft.Extensions.DependencyInjection;
using STAN.Client;

namespace HotBrokerBus.Stan.Injection.Options.Event.Subscription.Config
{
    public class StanEventBusSubscriptionConfigOptionsBuilder
    {

        private readonly List<StanEventEventSubscriptionParameters> _stanEventEventParameters;

        private readonly Dictionary<string, Tuple<Type, Type>> _typesToBind;

        private readonly IServiceCollection _serviceCollection;
        
        public StanEventBusSubscriptionConfigOptionsBuilder(List<StanEventEventSubscriptionParameters> stanEventEventParameters, IServiceCollection serviceCollection)
        {
            _stanEventEventParameters = stanEventEventParameters;

            _typesToBind = new();

            _serviceCollection = serviceCollection;
        }

        public StanEventBusSubscriptionConfigOptionsBuilder Bind<T, T2>()
            where T: IEvent
            where T2 : IEventHandler
        {
            var typeName = typeof(T).Name;

            if (!_typesToBind.ContainsKey(typeName))
            {
                _typesToBind.Add(typeName, new Tuple<Type, Type>(typeof(T), typeof(T2)));

                _serviceCollection.AddTransient(typeof(T2));
            }

            return this;
        }

        internal StanEventBusSubscriptionConfigOptions Build()
        {
            Dictionary<string, Tuple<Type, Type, string, StanSubscriptionOptions>> eventsMap = new();
            
            foreach (var @event in _stanEventEventParameters)
            {
                if (!_typesToBind.ContainsKey(@event.Type))
                {
                    throw new ArgumentException(
                        $"The type {@event.Type} is not bound. Add options.Bind<${@event.Type}>() after stanEventOptions.WithConfig().");
                }

                var subscriptionOptions = buildSubscriptionOptions(@event);

                var types = _typesToBind[@event.Type];
                
                eventsMap.Add(@event.SubscriptionName, new Tuple<Type, Type, string, StanSubscriptionOptions>(types.Item1, types.Item2, @event.QueueGroup, subscriptionOptions));
            }

            return new StanEventBusSubscriptionConfigOptions(eventsMap);
        }

        private StanSubscriptionOptions buildSubscriptionOptions(StanEventEventSubscriptionParameters @event)
        {
            var eventSubscriptionsOptions = @event.SubscriptionOptions;
            
            var subscriptionOptions = StanSubscriptionOptions.GetDefaultOptions();

            if (eventSubscriptionsOptions != null)
            {
                // Build behavior
                if (eventSubscriptionsOptions.Behavior ==
                    StanEventEventSubscriptionOptionsBehavior.DeliverAllAvailable)
                {
                    subscriptionOptions.DeliverAllAvailable();
                } else if (eventSubscriptionsOptions.Behavior ==
                           StanEventEventSubscriptionOptionsBehavior.StartWithLastReceived)
                {
                    subscriptionOptions.StartWithLastReceived();
                } else if (eventSubscriptionsOptions.Behavior ==
                           StanEventEventSubscriptionOptionsBehavior.StartAt)
                {
                    if (!String.IsNullOrEmpty(eventSubscriptionsOptions.BehaviorAdditional))
                    {
                        if (TimeSpan.TryParse(eventSubscriptionsOptions.BehaviorAdditional,
                            out var startAtTimespan))
                        {
                            subscriptionOptions.StartAt(startAtTimespan);
                        } else if (DateTime.TryParse(eventSubscriptionsOptions.BehaviorAdditional,
                            out var startAtDateTime))
                        {
                            subscriptionOptions.StartAt(startAtDateTime);
                        } else
                        {
                            throw new ArgumentException(
                                $"Unable to configure the event {@event.Type}. The configuration is set to StartAt but we can't parse either a timespan or a datetime.");
                        }
                    } else
                    {
                        throw new ArgumentException(
                            $"Unable to configure the event {@event.Type}. The configuration is set to StartAt but there's no or an empty behaviorAdditional key set.");
                    }
                }

                if (eventSubscriptionsOptions.AutoAck != null)
                {
                    subscriptionOptions.ManualAcks = eventSubscriptionsOptions.AutoAck ?? false;
                }

                if (eventSubscriptionsOptions.AckTimeout != null)
                {
                    subscriptionOptions.AckWait = eventSubscriptionsOptions.AckTimeout ?? 30000;
                }

                if (!String.IsNullOrEmpty(eventSubscriptionsOptions.DurableName))
                {
                    subscriptionOptions.DurableName = eventSubscriptionsOptions.DurableName;
                }
                
                if (eventSubscriptionsOptions.MaxInFlight != null)
                {
                    subscriptionOptions.MaxInflight = eventSubscriptionsOptions.MaxInFlight ?? StanConsts.DefaultMaxInflight;
                }

                if (eventSubscriptionsOptions.LeaveOpen != null)
                {
                    subscriptionOptions.LeaveOpen = eventSubscriptionsOptions.LeaveOpen ?? false;
                }
            }
            
            return subscriptionOptions;
        }
        
    }
}