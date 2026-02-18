using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Vehicles.Queries.GetVehicles;

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, Result<GetVehiclesResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetVehiclesQueryHandler> _logger;

    public GetVehiclesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetVehiclesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetVehiclesResponse, Success, Error>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        // Get all vehicles
        var allVehicles = await _unitOfWork.VehicleRepository.GetAllAsync();

        // Apply customer filter if provided
        if (request.CustomerId.HasValue)
            allVehicles = allVehicles.Where(v => v.CustomerId == request.CustomerId.Value);

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            allVehicles = allVehicles.Where(v => 
                v.Brand.ToLowerInvariant().Contains(searchTerm) ||
                v.Model.ToLowerInvariant().Contains(searchTerm) ||
                v.LicensePlate.ToLowerInvariant().Contains(searchTerm) ||
                v.Color.ToLowerInvariant().Contains(searchTerm) ||
                (v.ChassisNumber != null && v.ChassisNumber.ToLowerInvariant().Contains(searchTerm))
            );
        }

        // Calculate pagination
        var totalCount = allVehicles.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var pagedVehicles = allVehicles
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        // Map to Response objects
        var vehicleResponses = pagedVehicles.Select(v => new VehicleListItemResponse
        {
            Id = v.Id,
            CustomerId = v.CustomerId,
            CustomerName = v.Customer?.Name?.FullName ?? "Unknown",
            Brand = v.Brand,
            Model = v.Model,
            LicensePlate = v.LicensePlate,
            Color = v.Color,
            Year = v.Year,
            VIN = v.ChassisNumber,
            EngineType = v.EngineNumber,
            FuelType = v.FuelType.ToString(),
            CreatedAt = v.CreatedAt,
            UpdatedAt = v.UpdatedAt
        });

        var response = new GetVehiclesResponse
        {
            Vehicles = vehicleResponses,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };

        return response;
    }
}