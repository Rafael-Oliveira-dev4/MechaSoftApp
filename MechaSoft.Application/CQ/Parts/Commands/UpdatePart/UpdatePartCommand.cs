using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Parts.Commands.UpdatePart;

public record UpdatePartCommand(
    Guid Id,
    string Name,
    string Description,
    string Category,
    string? Brand,
    decimal UnitCost,
    decimal SalePrice,
    int MinStockLevel,
    string? SupplierName,
    string? SupplierContact,
    string? Location,
    bool IsActive
) : IRequest<Result<UpdatePartResponse, Success, Error>>;

public record UpdatePartResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    DateTime UpdatedAt
);

