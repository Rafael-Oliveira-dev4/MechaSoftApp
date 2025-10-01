using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Services.Commands.CreateService;

public record CreateServiceCommand(
    string Name,
    string Description,
    ServiceCategory Category,
    decimal EstimatedHours,
    decimal PricePerHour,
    decimal? FixedPrice,
    bool RequiresInspection
) : IRequest<Result<CreateServiceResponse, Success, Error>>;

public record CreateServiceResponse(
    Guid Id,
    string Name,
    string Description,
    ServiceCategory Category,
    decimal EstimatedHours,
    decimal PricePerHour,
    decimal? FixedPrice,
    DateTime CreatedAt
);

