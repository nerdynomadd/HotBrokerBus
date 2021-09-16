using System.Collections.Generic;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Stan.SubscriptionStorage;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan
{
    public interface IStanBusSubscriptionsStorage
    {
        void AddEventSubscription<T, TH>(string subscriptionName, string subject, string queueGroup,
            StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler;

        void AddCommandSubscription<T, TH>(string subscriptionName, string subject)
            where T : ICommand
            where TH : ICommandHandler;

        public List<StanSubscriptionInfo> RetrieveCommandSubscriptions();

        public List<StanSubscriptionInfo> RetrieveEventSubscriptions();


        void RemoveSubscription(string subscriptionName);

        bool HasSubscription(string susbcriptionName);
    }
}