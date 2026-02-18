using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Services.Queries.GetServiceById;

public record GetServiceByIdQuery(Guid Id) : IRequest<Result<ServiceResponse, Success, Error>>;

public record ServiceResponse(
    Guid Id,
    string Name,
    string Description,
    Domain.Model.ServiceCategory Category,
    decimal EstimatedHours,
    decimal PricePerHour,
    decimal? FixedPrice,
    bool IsActive,
    bool RequiresInspection,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);

