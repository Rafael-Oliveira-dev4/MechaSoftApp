using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Reports.Queries.GetLowStockReport;

public class GetLowStockReportQueryHandler : IRequestHandler<GetLowStockReportQuery, Result<LowStockReportResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLowStockReportQueryHandler> _logger;

    public GetLowStockReportQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLowStockReportQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LowStockReportResponse, Success, Error>> Handle(GetLowStockReportQuery request, CancellationToken cancellationToken)
    {
        var allParts = await _unitOfWork.PartRepository.GetAllAsync();

        var lowStockParts = allParts
            .Where(p => p.IsLowStock() && p.IsActive)
            .OrderBy(p => p.StockQuantity - p.MinStockLevel) // Most critical first
            .ToList();

        var partDetails = lowStockParts.Select(p => new LowStockPartDetail(
            p.Id,
            p.Code,
            p.Name,
            p.Category,
            p.StockQuantity,
            p.MinStockLevel,
            p.MinStockLevel - p.StockQuantity,
            p.UnitCost.Amount,
            p.SalePrice.Amount,
            (p.MinStockLevel - p.StockQuantity) * p.UnitCost.Amount, // Potential lost sales
            p.SupplierName,
            p.SupplierContact,
            p.Location
        )).ToList();

        var totalValueAtRisk = partDetails.Sum(p => p.ValueAtRisk);

        var response = new LowStockReportResponse(
            lowStockParts.Count,
            totalValueAtRisk,
            partDetails,
            DateTime.UtcNow
        );

        _logger.LogInformation("Low stock report generated: {Count} parts at low stock, Value at risk: {Value:C}", 
            lowStockParts.Count, totalValueAtRisk);

        return response;
    }
}

