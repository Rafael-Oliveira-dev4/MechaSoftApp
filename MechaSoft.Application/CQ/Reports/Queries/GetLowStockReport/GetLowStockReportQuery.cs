using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Reports.Queries.GetLowStockReport;

/// <summary>
/// Detailed low stock report for inventory management
/// </summary>
public record GetLowStockReportQuery() : IRequest<Result<LowStockReportResponse, Success, Error>>;

public record LowStockReportResponse(
    int TotalLowStockParts,
    decimal TotalValueAtRisk,
    List<LowStockPartDetail> Parts,
    DateTime GeneratedAt
);

public record LowStockPartDetail(
    Guid PartId,
    string Code,
    string Name,
    string Category,
    int CurrentStock,
    int MinStock,
    int Deficit,
    decimal UnitCost,
    decimal SalePrice,
    decimal ValueAtRisk,
    string? SupplierName,
    string? SupplierContact,
    string? Location
);

