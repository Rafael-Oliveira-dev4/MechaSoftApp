using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.CreateServiceOrder;

public record CreateServiceOrderCommand(
    Guid CustomerId,
    Guid VehicleId,
    string Description,
    Priority Priority,
    decimal EstimatedCost,
    DateTime? EstimatedDelivery,
    Guid? MechanicId,
    bool RequiresInspection,
    string? InternalNotes
) : IRequest<Result<CreateServiceOrderResponse, Success, Error>>;

public record CreateServiceOrderResponse(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    Guid VehicleId,
    ServiceOrderStatus Status,
    DateTime CreatedAt
);

