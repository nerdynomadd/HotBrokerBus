using System.Collections.Generic;
using Bus.Abstractions.Commands;
using Bus.Abstractions.Events;
using Bus.Stan.SubscriptionStorage;
using STAN.Client;

namespace Bus.Abstractions.Stan
{
    public interface IStanBusSubscriptionsStorage
    {
        void AddEventSubscription<T, TH>(string subscriptionName, string subject, string triggerName, string queueGroup,
            StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler;

        void AddCommandSubscription<T, TH>(string subscriptionName, string subject, string triggerName)
            where T : ICommand
            where TH : ICommandHandler;

        public List<StanSubscriptionInfo> RetrieveCommandSubscriptions();

        public List<StanSubscriptionInfo> RetrieveEventSubscriptions();


        void RemoveSubscription(string subscriptionName);

        bool HasSubscription(string susbcriptionName);
    }
}