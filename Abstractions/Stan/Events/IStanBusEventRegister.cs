using HotBrokerBus.Abstractions.Events;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Events
{
    public interface IStanBusEventRegister
    {
        public IStanConnection Connection { get; set; }

        internal void Resume();

        internal void SetConnection(IStanConnection connection);

        void Publish(string subject, IEvent @event);

        void Publish(string subject, string eventName, IEvent @event);

        void Subscribe<T, TH>(string subject, string queueGroup, StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler<T>;

        void Subscribe<T, TH>(string subject, string eventName, string queueGroup,
            StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler<T>;

        /*
         * void SubscribeDynamic<TH>(string subject, string eventName, string queueGroup, StanSubscriptionOptions subscriptionOptions)
            where TH : IDynamicIntegrationEventHandler;
         */

        void Close<T, TH>(string subject)
            where T : IEvent
            where TH : IEventHandler<T>;

        void CloseAll();

        void Unsubscribe<T, TH>(string subject)
            where T : IEvent
            where TH : IEventHandler<T>;

        void UnsubscribeAll();

        /*
         * void UnsubscribeDynamic<TH>(string subject, string eventName)
            where TH : IDynamicIntegrationEventHandler;
         */
    }
}