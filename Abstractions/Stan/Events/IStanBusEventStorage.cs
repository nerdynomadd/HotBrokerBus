using System;
using System.Collections.Generic;
using Bus.Abstractions.Events;

namespace Bus.Abstractions.Stan.Events
{
    public interface IStanBusEventStorage
    {
        public void AddEventMessage(string subject, string eventName, IEvent @event);

        public void RemoveEventMessage(Tuple<string, string, IEvent> message);

        public void ClearMessages();

        public List<Tuple<string, string, IEvent>> RetrieveEventMessages();
    }
}