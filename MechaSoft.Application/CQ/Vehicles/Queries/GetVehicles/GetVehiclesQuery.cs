using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Vehicles.Queries.GetVehicles;

public class GetVehiclesQuery : IRequest<Result<GetVehiclesResponse, Success, Error>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CustomerId { get; set; }
    public string? SearchTerm { get; set; }
}

public class GetVehiclesResponse
{
    public required IEnumerable<VehicleResponse> Vehicles { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

public class VehicleResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public required string CustomerName { get; set; }
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