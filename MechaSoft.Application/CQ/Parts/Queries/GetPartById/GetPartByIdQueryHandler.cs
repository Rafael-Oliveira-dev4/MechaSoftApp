using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Parts.Queries.GetPartById;

public class GetPartByIdQueryHandler : IRequestHandler<GetPartByIdQuery, Result<PartResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPartByIdQueryHandler> _logger;

    public GetPartByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPartByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PartResponse, Success, Error>> Handle(GetPartByIdQuery request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.PartRepository.GetByIdAsync(request.Id);
        if (part == null)
        {
            _logger.LogWarning("Part not found: {PartId}", request.Id);
            return Error.PartNotFound;
        }

        var response = new PartResponse(
            part.Id,
            part.Code,
            part.Name,
            part.Description,
            part.Category,
            part.Brand,
            part.StockQuantity,
            part.MinStockLevel,
            part.IsLowStock(),
            part.UnitCost.Amount,
            part.SalePrice.Amount,
            part.SupplierName,
            part.SupplierContact,
            part.Location,
            part.IsActive,
            part.CreatedAt,
            part.UpdatedAt
        );

        return response;
    }
}
