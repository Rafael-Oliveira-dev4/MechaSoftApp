using MechaSoft.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Events;

public class ServiceOrderStatusChangedEvent : DomainEvent
{
    public Guid ServiceOrderId { get; private set; }
    public ServiceOrderStatus PreviousStatus { get; private set; }
    public ServiceOrderStatus NewStatus { get; private set; }
    public string? Notes { get; private set; }

    public ServiceOrderStatusChangedEvent(Guid serviceOrderId, ServiceOrderStatus previousStatus,
                                        ServiceOrderStatus newStatus, string? notes = null)
    {
        ServiceOrderId = serviceOrderId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        Notes = notes;
    }
}
