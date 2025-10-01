using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Services.Queries.GetServices;

public record GetServicesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    ServiceCategory? Category = null,
    bool? IsActive = null,
    string? SearchTerm = null
) : IRequest<Result<GetServicesResponse, Success, Error>>;

public record GetServicesResponse(
    IEnumerable<ServiceDto> Services,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public record ServiceDto(
    Guid Id,
    string Name,
    string Description,
    ServiceCategory Category,
    decimal EstimatedHours,
    decimal PricePerHour,
    decimal? FixedPrice,
    bool IsActive,
    bool RequiresInspection
);

