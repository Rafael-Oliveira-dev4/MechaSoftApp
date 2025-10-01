using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Parts.Commands.CreatePart;

public record CreatePartCommand(
    string Code,
    string Name,
    string Description,
    string Category,
    string? Brand,
    decimal UnitCost,
    decimal SalePrice,
    int StockQuantity,
    int MinStockLevel,
    string? SupplierName,
    string? SupplierContact,
    string? Location
) : IRequest<Result<CreatePartResponse, Success, Error>>;

public record CreatePartResponse(
    Guid Id,
    string Code,
    string Name,
    string Category,
    decimal UnitCost,
    decimal SalePrice,
    int StockQuantity,
    DateTime CreatedAt
);

