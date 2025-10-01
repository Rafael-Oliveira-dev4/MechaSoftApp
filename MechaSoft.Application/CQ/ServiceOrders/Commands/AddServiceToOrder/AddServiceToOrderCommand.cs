using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.AddServiceToOrder;

public record AddServiceToOrderCommand(
    Guid ServiceOrderId,
    Guid ServiceId,
    int Quantity,
    decimal EstimatedHours,
    decimal? DiscountPercentage,
    Guid? MechanicId
) : IRequest<Result<AddServiceToOrderResponse, Success, Error>>;

public record AddServiceToOrderResponse(
    Guid ServiceOrderId,
    Guid ServiceItemId,
    string ServiceName,
    decimal TotalPrice
);

