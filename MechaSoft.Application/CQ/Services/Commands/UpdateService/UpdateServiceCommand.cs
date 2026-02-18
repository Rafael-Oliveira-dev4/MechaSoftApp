using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Services.Commands.UpdateService;

public record UpdateServiceCommand(
    Guid Id,
    string Name,
    string Description,
    ServiceCategory Category,
    decimal EstimatedHours,
    decimal PricePerHour,
    decimal? FixedPrice,
    bool RequiresInspection,
    bool IsActive
) : IRequest<Result<UpdateServiceResponse, Success, Error>>;

public record UpdateServiceResponse(
    Guid Id,
    string Name,
    bool IsActive,
    DateTime UpdatedAt
);

