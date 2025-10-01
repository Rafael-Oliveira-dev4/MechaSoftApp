using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Inspections.Commands.CreateInspection;

public class CreateInspectionCommandHandler : IRequestHandler<CreateInspectionCommand, Result<CreateInspectionResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateInspectionCommandHandler> _logger;

    public CreateInspectionCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateInspectionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreateInspectionResponse, Success, Error>> Handle(CreateInspectionCommand request, CancellationToken cancellationToken)
    {
        // Validate vehicle exists
        var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
        {
            _logger.LogWarning("Attempt to create inspection for non-existent vehicle: {VehicleId}", request.VehicleId);
            return Error.VehicleNotFound;
        }

        // Validate service order exists
        var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (serviceOrder == null)
        {
            _logger.LogWarning("Attempt to create inspection for non-existent service order: {ServiceOrderId}", request.ServiceOrderId);
            return Error.ServiceOrderNotFound;
        }

        var cost = new Money(request.Cost, "EUR");

        // Create inspection
        var inspection = new Inspection(
            request.VehicleId,
            request.ServiceOrderId,
            request.Type,
            request.InspectionDate,
            request.ExpiryDate,
            cost,
            request.InspectionCenter,
            request.VehicleMileage
        );

        inspection.Observations = request.Observations;

        var savedInspection = await _unitOfWork.InspectionRepository.SaveAsync(inspection);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Inspection created successfully: {InspectionId}, Vehicle: {VehicleId}, Type: {Type}", 
            savedInspection.Id, request.VehicleId, request.Type);

        var response = new CreateInspectionResponse(
            savedInspection.Id,
            savedInspection.VehicleId,
            savedInspection.Type,
            savedInspection.InspectionDate,
            savedInspection.Result,
            savedInspection.CreatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}
