using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Queries.CheckEmailAvailability;

public record CheckEmailAvailabilityQuery(string Email) : IRequest<Result<CheckEmailAvailabilityResponse>>;

public record CheckEmailAvailabilityResponse(bool IsAvailable);

