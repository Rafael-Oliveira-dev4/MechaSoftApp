using MechaSoft.Domain.Model;

namespace MechaSoft.Application.CQ.ServiceOrders.Common;

public record CreateServiceOrderRequest(
    Guid CustomerId,
    Guid VehicleId,
    string Description,
    Priority Priority,
    decimal EstimatedCost,
    DateTime? EstimatedDelivery,
    Guid? MechanicId,
    bool RequiresInspection,
    string? InternalNotes
);

public record UpdateStatusRequest(
    ServiceOrderStatus Status,
    string? Notes
);

public record AssignMechanicRequest(
    Guid MechanicId
);

public record AddServiceRequest(
    Guid ServiceId,
    int Quantity,
    decimal EstimatedHours,
    decimal? DiscountPercentage,
    Guid? MechanicId
);

public record AddPartRequest(
    Guid PartId,
    int Quantity,
    decimal? DiscountPercentage
);


