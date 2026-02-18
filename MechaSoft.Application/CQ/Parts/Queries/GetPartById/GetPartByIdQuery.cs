using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Parts.Queries.GetPartById;

public record GetPartByIdQuery(Guid Id) : IRequest<Result<PartResponse, Success, Error>>;

public record PartResponse(
    Guid Id,
    string Code,
    string Name,
    string Description,
    string Category,
    string? Brand,
    int StockQuantity,
    int MinStockLevel,
    bool IsLowStock,
    decimal UnitCost,
    decimal SalePrice,
    string? SupplierName,
    string? SupplierContact,
    string? Location,
    bool IsActive,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);

