using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Commands.RefreshToken;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<Result<RefreshTokenResponse, Success, Error>>;

public record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
