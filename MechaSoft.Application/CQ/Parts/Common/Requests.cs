using MechaSoft.Domain.Model;

namespace MechaSoft.Application.CQ.Parts.Common;

public record CreatePartRequest(
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
);

public record UpdateStockRequest(
    int Quantity,
    StockMovementType MovementType,
    string? Reason
);


