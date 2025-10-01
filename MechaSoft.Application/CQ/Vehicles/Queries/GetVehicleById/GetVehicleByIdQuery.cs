using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Vehicles.Queries.GetVehicleById;

public record GetVehicleByIdQuery(Guid Id) : IRequest<Result<VehicleDetailsResponse, Success, Error>>;

public record VehicleDetailsResponse(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string Brand,
    string Model,
    int Year,
    string LicensePlate,
    string Color,
    string FuelType,
    int? Mileage,
    string? ChassisNumber,
    string? EngineNumber,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);

