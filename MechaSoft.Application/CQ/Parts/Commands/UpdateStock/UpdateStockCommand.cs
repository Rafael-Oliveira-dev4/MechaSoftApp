using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Parts.Commands.UpdateStock;

public record UpdateStockCommand(
    Guid PartId,
    int Quantity,
    StockMovementType MovementType,
    string? Reason
) : IRequest<Result<UpdateStockResponse, Success, Error>>;

public record UpdateStockResponse(
    Guid PartId,
    int NewStockQuantity,
    bool IsLowStock,
    string Message
);

