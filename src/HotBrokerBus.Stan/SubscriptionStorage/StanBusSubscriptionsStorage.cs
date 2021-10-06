using System.Collections.Generic;
using System.Linq;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Subscription;
using STAN.Client;

namespace HotBrokerBus.Stan.SubscriptionStorage
{
    public class StanBusSubscriptionsStorage : IStanBusSubscriptionsStorage
    {
        private readonly IList<IStanSubscriptionInfo> _subscriptionInfos;

        public StanBusSubscriptionsStorage()
        {
            _subscriptionInfos = new List<IStanSubscriptionInfo>();
        }

        public void AddEventSubscription<T, TH>(string subscriptionName, string subject,
            string queueGroup, StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler
        {
            _subscriptionInfos.Add(new StanSubscriptionInfo(subscriptionName, subject, queueGroup,
                IStanSubscriptionInfo.Types.Event, subscriptionOptions, typeof(T), typeof(TH)));
        }

        public void AddCommandSubscription<T, TH>(string subscriptionName, string subject)
            where T : ICommand
            where TH : ICommandHandler
        {
            _subscriptionInfos.Add(new StanSubscriptionInfo(subscriptionName, subject,
                IStanSubscriptionInfo.Types.Command, typeof(T), typeof(TH)));
        }

        public List<IStanSubscriptionInfo> RetrieveCommandSubscriptions()
        {
            return _subscriptionInfos.Where(e => e.Type == IStanSubscriptionInfo.Types.Command).ToList();
        }

        public List<IStanSubscriptionInfo> RetrieveEventSubscriptions()
        {
            return _subscriptionInfos.Where(e => e.Type == IStanSubscriptionInfo.Types.Event).ToList();
        }

        public void RemoveSubscription(string subscriptionName)
        {
            _subscriptionInfos.Remove(_subscriptionInfos.First(e => e.SubscriptionName == subscriptionName));
        }

        public bool HasSubscription(string subscriptionName)
        {
            return _subscriptionInfos.Any(e => e.SubscriptionName == subscriptionName);
        }
    }
}