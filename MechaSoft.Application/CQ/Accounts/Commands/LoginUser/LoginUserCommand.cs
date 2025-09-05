using MechaSoft.Domain.Model;
using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Commands.LoginUser;

public record LoginUserCommand(
    string Username,
    string Password
) : IRequest<Result<LoginUserResponse, Success, Error>>;

public record LoginUserResponse(
    Guid UserId,
    string Username,
    string Email,
    UserRole Role,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
