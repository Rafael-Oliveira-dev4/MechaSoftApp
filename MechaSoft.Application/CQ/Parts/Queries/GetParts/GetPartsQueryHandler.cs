using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Parts.Queries.GetParts;

public class GetPartsQueryHandler : IRequestHandler<GetPartsQuery, Result<GetPartsResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPartsQueryHandler> _logger;

    public GetPartsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPartsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetPartsResponse, Success, Error>> Handle(GetPartsQuery request, CancellationToken cancellationToken)
    {
        var allParts = await _unitOfWork.PartRepository.GetAllAsync();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Category))
            allParts = allParts.Where(p => p.Category == request.Category);

        if (request.IsActive.HasValue)
            allParts = allParts.Where(p => p.IsActive == request.IsActive.Value);

        if (request.LowStockOnly == true)
            allParts = allParts.Where(p => p.IsLowStock());

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            allParts = allParts.Where(p =>
                p.Code.ToLowerInvariant().Contains(searchTerm) ||
                p.Name.ToLowerInvariant().Contains(searchTerm) ||
                p.Description.ToLowerInvariant().Contains(searchTerm)
            );
        }

        // Pagination
        var totalCount = allParts.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var pagedParts = allParts
            .OrderBy(p => p.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var partDtos = pagedParts.Select(p => new PartDto(
            p.Id,
            p.Code,
            p.Name,
            p.Description,
            p.Category,
            p.Brand,
            p.StockQuantity,
            p.MinStockLevel,
            p.IsLowStock(),
            p.UnitCost.Amount,
            p.SalePrice.Amount,
            p.SupplierName,
            p.Location,
            p.IsActive
        ));

        var response = new GetPartsResponse(
            partDtos,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );

        return response;
    }
}
