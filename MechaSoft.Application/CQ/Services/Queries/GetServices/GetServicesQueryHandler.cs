using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Services.Queries.GetServices;

public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, Result<GetServicesResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetServicesQueryHandler> _logger;

    public GetServicesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetServicesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetServicesResponse, Success, Error>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        var allServices = await _unitOfWork.ServiceRepository.GetAllAsync();

        // Apply filters
        if (request.Category.HasValue)
            allServices = allServices.Where(s => s.Category == request.Category.Value);

        if (request.IsActive.HasValue)
            allServices = allServices.Where(s => s.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            allServices = allServices.Where(s =>
                s.Name.ToLowerInvariant().Contains(searchTerm) ||
                s.Description.ToLowerInvariant().Contains(searchTerm)
            );
        }

        // Pagination
        var totalCount = allServices.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var pagedServices = allServices
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var serviceDtos = pagedServices.Select(s => new ServiceDto(
            s.Id,
            s.Name,
            s.Description,
            s.Category,
            s.EstimatedHours,
            s.PricePerHour.Amount,
            s.FixedPrice?.Amount,
            s.IsActive,
            s.RequiresInspection
        ));

        var response = new GetServicesResponse(
            serviceDtos,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );

        return response;
    }
}

