using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Vehicles.Commands.UpdateVehicle;

public record UpdateVehicleCommand(
    Guid Id,
    string Brand,
    string Model,
    int Year,
    string LicensePlate,
    string Color,
    string FuelType,
    int? Mileage,
    string? ChassisNumber,
    string? EngineNumber
) : IRequest<Result<UpdateVehicleResponse, Success, Error>>;

public record UpdateVehicleResponse(
    Guid Id,
    string Brand,
    string Model,
    string LicensePlate,
    DateTime UpdatedAt
);

