using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Vehicles.Queries.GetVehicleById;

public record GetVehicleByIdQuery(Guid Id) : IRequest<Result<VehicleResponse, Success, Error>>;

public record VehicleResponse(
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

