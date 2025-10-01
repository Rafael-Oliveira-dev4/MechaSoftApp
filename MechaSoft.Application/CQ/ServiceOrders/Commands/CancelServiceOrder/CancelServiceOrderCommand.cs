using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.CancelServiceOrder;

/// <summary>
/// Cancel service order - preserves record but marks as cancelled
/// Returns parts to stock if already allocated
/// </summary>
public record CancelServiceOrderCommand(
    Guid ServiceOrderId,
    string Reason
) : IRequest<Result<CancelServiceOrderResponse, Success, Error>>;

public record CancelServiceOrderResponse(
    Guid ServiceOrderId,
    string OrderNumber,
    int PartsReturnedToStock,
    string Message
);

