using System;
using Bus.Abstractions.Events;

namespace Bus.Events
{
    public class Event : IEvent
    {
        public Guid Id { get; } = new();

        public DateTime CreationDate { get; } = DateTime.Now;
    }
}