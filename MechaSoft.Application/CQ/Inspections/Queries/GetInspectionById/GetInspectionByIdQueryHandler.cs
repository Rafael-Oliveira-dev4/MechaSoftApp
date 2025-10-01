using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Inspections.Queries.GetInspectionById;

public class GetInspectionByIdQueryHandler : IRequestHandler<GetInspectionByIdQuery, Result<InspectionResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetInspectionByIdQueryHandler> _logger;

    public GetInspectionByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetInspectionByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<InspectionResponse, Success, Error>> Handle(GetInspectionByIdQuery request, CancellationToken cancellationToken)
    {
        var inspection = await _unitOfWork.InspectionRepository.GetByIdAsync(request.Id);
        if (inspection == null)
        {
            _logger.LogWarning("Inspection not found: {InspectionId}", request.Id);
            return Error.InspectionNotFound;
        }

        // Get related entities
        var vehicle = inspection.Vehicle ?? await _unitOfWork.VehicleRepository.GetByIdAsync(inspection.VehicleId);
        var serviceOrder = inspection.ServiceOrder ?? await _unitOfWork.ServiceOrderRepository.GetByIdAsync(inspection.ServiceOrderId);

        var response = new InspectionResponse(
            inspection.Id,
            inspection.VehicleId,
            vehicle != null ? $"{vehicle.Brand} {vehicle.Model} - {vehicle.LicensePlate}" : "Unknown",
            inspection.ServiceOrderId,
            serviceOrder?.OrderNumber ?? "Unknown",
            inspection.Type,
            inspection.InspectionDate,
            inspection.ExpiryDate,
            inspection.Result,
            inspection.Observations,
            inspection.Cost.Amount,
            inspection.CertificateNumber,
            inspection.InspectionCenter,
            inspection.VehicleMileage,
            inspection.CreatedAt,
            inspection.UpdatedAt
        );

        return response;
    }
}
