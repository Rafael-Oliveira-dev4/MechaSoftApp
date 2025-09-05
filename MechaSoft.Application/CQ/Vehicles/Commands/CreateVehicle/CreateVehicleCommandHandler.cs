using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateVehicleCommand, Result<CreateVehicleResponse, Success, Error>>
{
    public async Task<Result<CreateVehicleResponse, Success, Error>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if customer exists
            var customer = await unitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
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
            var savedVehicle = await unitOfWork.VehicleRepository.SaveAsync(vehicle);
            await unitOfWork.CommitAsync(cancellationToken);

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
        catch (Exception)
        {
            return Error.OperationFailed;
        }
    }
}