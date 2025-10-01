using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.AddPartToOrder;

public record AddPartToOrderCommand(
    Guid ServiceOrderId,
    Guid PartId,
    int Quantity,
    decimal? DiscountPercentage
) : IRequest<Result<AddPartToOrderResponse, Success, Error>>;

public record AddPartToOrderResponse(
    Guid ServiceOrderId,
    Guid PartId,
    string PartName,
    int Quantity,
    decimal TotalPrice,
    int RemainingStock
);

