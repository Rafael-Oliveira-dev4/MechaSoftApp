using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Result<CreateVehicleResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVehicleCommandHandler> _logger;

    public CreateVehicleCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateVehicleCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreateVehicleResponse, Success, Error>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        // Check if customer exists
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            _logger.LogWarning("Attempt to create vehicle for non-existent customer: {CustomerId}", request.CustomerId);
            return Error.CustomerNotFound;
        }

        // Create vehicle using object initializer
        var vehicle = new Vehicle
        {
            CustomerId = request.CustomerId,
            Brand = request.Brand,
            Model = request.Model,
            Year = request.Year,
            LicensePlate = request.LicensePlate,
            Color = request.Color,
            FuelType = request.FuelType,
            Mileage = null,
            ChassisNumber = request.VIN,
            EngineNumber = request.EngineType,
            Customer = customer
        };

        // Save vehicle
        var savedVehicle = await _unitOfWork.VehicleRepository.SaveAsync(vehicle);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Vehicle created successfully: {VehicleId}, {LicensePlate}", savedVehicle.Id, savedVehicle.LicensePlate);

        // Create response
        var response = new CreateVehicleResponse
        {
            Id = savedVehicle.Id,
            CustomerId = savedVehicle.CustomerId,
            Brand = savedVehicle.Brand,
            Model = savedVehicle.Model,
            LicensePlate = savedVehicle.LicensePlate,
            Color = savedVehicle.Color,
            Year = savedVehicle.Year,
            VIN = savedVehicle.ChassisNumber,
            EngineType = savedVehicle.EngineNumber,
            FuelType = savedVehicle.FuelType.ToString(),
            CreatedAt = savedVehicle.CreatedAt,
            UpdatedAt = savedVehicle.UpdatedAt
        };

        return response;
    }
}