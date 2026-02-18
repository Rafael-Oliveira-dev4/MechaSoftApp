using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Vehicles.Queries.GetVehicleById;

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, Result<VehicleDetailsResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetVehicleByIdQueryHandler> _logger;

    public GetVehicleByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetVehicleByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<VehicleDetailsResponse, Success, Error>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(request.Id);
        if (vehicle == null)
        {
            _logger.LogWarning("Vehicle not found: {VehicleId}", request.Id);
            return Error.VehicleNotFound;
        }

        // Get customer name if customer exists
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(vehicle.CustomerId);
        var customerName = customer?.Name.FullName ?? "Unknown";

        var response = new VehicleDetailsResponse(
            vehicle.Id,
            vehicle.CustomerId,
            customerName,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Year,
            vehicle.LicensePlate,
            vehicle.Color,
            vehicle.FuelType.ToString(),
            vehicle.Mileage,
            vehicle.ChassisNumber,
            vehicle.EngineNumber,
            vehicle.CreatedAt,
            vehicle.UpdatedAt
        );

        return response;
    }
}

