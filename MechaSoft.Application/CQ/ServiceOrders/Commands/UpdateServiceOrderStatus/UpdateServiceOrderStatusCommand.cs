using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.UpdateServiceOrderStatus;

public record UpdateServiceOrderStatusCommand(
    Guid ServiceOrderId,
    ServiceOrderStatus NewStatus,
    string? Notes
) : IRequest<Result<UpdateServiceOrderStatusResponse, Success, Error>>;

public record UpdateServiceOrderStatusResponse(
    Guid Id,
    string OrderNumber,
    ServiceOrderStatus Status,
    DateTime UpdatedAt
);

