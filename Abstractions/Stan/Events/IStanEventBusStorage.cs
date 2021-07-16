using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Events;

namespace HotBrokerBus.Abstractions.Stan.Events
{
    public interface IStanEventBusStorage
    {
        public void AddEventMessage(string subject, IEvent @event);

        public void RemoveEventMessage(Tuple<string, IEvent> message);

        public void ClearMessages();

        public List<Tuple<string, IEvent>> RetrieveEventMessages();
    }
}