using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Parts.Queries.GetParts;

public record GetPartsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Category = null,
    bool? IsActive = null,
    bool? LowStockOnly = null,
    string? SearchTerm = null
) : IRequest<Result<GetPartsResponse, Success, Error>>;

public record GetPartsResponse(
    IEnumerable<PartDto> Parts,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public record PartDto(
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
    string? Location,
    bool IsActive
);

