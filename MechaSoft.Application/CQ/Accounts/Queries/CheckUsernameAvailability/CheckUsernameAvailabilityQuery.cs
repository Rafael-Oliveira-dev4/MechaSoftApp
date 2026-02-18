using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Accounts.Queries.CheckUsernameAvailability;

public record CheckUsernameAvailabilityQuery(string Username) : IRequest<Result<CheckUsernameAvailabilityResponse>>;

