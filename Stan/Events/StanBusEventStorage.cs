using System;
using System.Collections.Generic;
using System.Linq;
using Bus.Abstractions.Events;
using Bus.Abstractions.Stan.Events;

namespace Bus.Stan.Events
{
    public class StanBusEventStorage : IStanBusEventStorage
    {
        private readonly List<Tuple<string, string, IEvent>> _messages;

        public StanBusEventStorage()
        {
            _messages = new List<Tuple<string, string, IEvent>>();
        }

        public void AddEventMessage(string subject, string eventName, IEvent @event)
        {
            _messages.Add(new Tuple<string, string, IEvent>(subject, eventName, @event));
        }

        public void RemoveEventMessage(Tuple<string, string, IEvent> message)
        {
            _messages.Remove(message);
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }

        public List<Tuple<string, string, IEvent>> RetrieveEventMessages()
        {
            return _messages.ToList();
        }
    }
}