using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Inspections.Queries.GetInspections;

public class GetInspectionsQueryHandler : IRequestHandler<GetInspectionsQuery, Result<GetInspectionsResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetInspectionsQueryHandler> _logger;

    public GetInspectionsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetInspectionsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetInspectionsResponse, Success, Error>> Handle(GetInspectionsQuery request, CancellationToken cancellationToken)
    {
        var allInspections = await _unitOfWork.InspectionRepository.GetAllAsync();

        // Apply filters
        if (request.VehicleId.HasValue)
            allInspections = allInspections.Where(i => i.VehicleId == request.VehicleId.Value);

        if (request.Result.HasValue)
            allInspections = allInspections.Where(i => i.Result == request.Result.Value);

        // Pagination
        var totalCount = allInspections.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var pagedInspections = allInspections
            .OrderByDescending(i => i.InspectionDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var inspectionDtos = pagedInspections.Select(i => new InspectionDto(
            i.Id,
            i.VehicleId,
            i.Vehicle != null ? $"{i.Vehicle.Brand} {i.Vehicle.Model} - {i.Vehicle.LicensePlate}" : "Unknown",
            i.Type,
            i.InspectionDate,
            i.ExpiryDate,
            i.Result,
            i.Cost.Amount,
            i.InspectionCenter,
            i.VehicleMileage,
            i.CertificateNumber
        ));

        var response = new GetInspectionsResponse(
            inspectionDtos,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );

        return response;
    }
}
