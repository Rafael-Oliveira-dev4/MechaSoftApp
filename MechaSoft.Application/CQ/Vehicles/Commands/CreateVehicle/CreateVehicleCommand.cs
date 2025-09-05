using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommand : IRequest<Result<CreateVehicleResponse, Success, Error>>
{
    public Guid CustomerId { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string LicensePlate { get; set; }
    public required string Color { get; set; }
    public int Year { get; set; }
    public string? VIN { get; set; }
    public string? EngineType { get; set; }
    public FuelType FuelType { get; set; }
}

public class CreateVehicleResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string LicensePlate { get; set; }
    public required string Color { get; set; }
    public int Year { get; set; }
    public string? VIN { get; set; }
    public string? EngineType { get; set; }
    public required string FuelType { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}