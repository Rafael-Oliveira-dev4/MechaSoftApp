namespace MechaSoft.Application.CQ.Vehicles.Common;

public record CreateVehicleRequest(
    Guid CustomerId,
    string Brand,
    string Model,
    string LicensePlate,
    string Color,
    int Year,
    string? VIN,
    string? EngineType,
    string FuelType
);

public record UpdateVehicleRequest(
    string Brand,
    string Model,
    int Year,
    string LicensePlate,
    string Color,
    string FuelType,
    int? Mileage,
    string? ChassisNumber,
    string? EngineNumber
);


