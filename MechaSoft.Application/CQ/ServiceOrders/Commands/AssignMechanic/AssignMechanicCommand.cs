using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.AssignMechanic;

public record AssignMechanicCommand(
    Guid ServiceOrderId,
    Guid MechanicId
) : IRequest<Result<AssignMechanicResponse, Success, Error>>;

public record AssignMechanicResponse(
    Guid ServiceOrderId,
    string OrderNumber,
    Guid MechanicId,
    string MechanicName
);

