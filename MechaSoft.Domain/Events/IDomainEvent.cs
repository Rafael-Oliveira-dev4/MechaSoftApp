using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; private set; }
    public Guid EventId { get; private set; }

    protected DomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
        EventId = Guid.NewGuid();
    }
}