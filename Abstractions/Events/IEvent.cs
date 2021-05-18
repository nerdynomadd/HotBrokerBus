using System;

namespace Bus.Abstractions.Events
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTime CreationDate { get; }
    }
}