using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Accounts.Queries.SuggestUsername;

public record SuggestUsernameQuery(string Username) : IRequest<Result<SuggestUsernameResponse>>;

