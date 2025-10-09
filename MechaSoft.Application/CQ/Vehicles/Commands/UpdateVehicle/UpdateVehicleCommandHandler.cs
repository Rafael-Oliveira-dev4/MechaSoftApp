using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, Result<UpdateVehicleResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVehicleCommandHandler> _logger;

    public UpdateVehicleCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateVehicleCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateVehicleResponse, Success, Error>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(request.Id);
        if (vehicle == null)
        {
            _logger.LogWarning("Attempt to update non-existent vehicle: {VehicleId}", request.Id);
            return Error.VehicleNotFound;
        }

        // Update properties
        vehicle.Brand = request.Brand;
        vehicle.Model = request.Model;
        vehicle.Year = request.Year;
        vehicle.LicensePlate = request.LicensePlate;
        vehicle.Color = request.Color;
        
        if (Enum.TryParse<FuelType>(request.FuelType, out var fuelType))
            vehicle.FuelType = fuelType;

        vehicle.Mileage = request.Mileage;
        vehicle.ChassisNumber = request.ChassisNumber;
        vehicle.EngineNumber = request.EngineNumber;

        await _unitOfWork.VehicleRepository.UpdateAsync(vehicle);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Vehicle updated successfully: {VehicleId}, {LicensePlate}", vehicle.Id, vehicle.LicensePlate);

        var response = new UpdateVehicleResponse(
            vehicle.Id,
            vehicle.Brand,
            vehicle.Model,
            vehicle.LicensePlate,
            vehicle.UpdatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

