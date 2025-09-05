using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Commands.ChangePassword;

public record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest<Result<ChangePasswordResponse, Success, Error>>;

public record ChangePasswordResponse(
    bool Success,
    string Message
);
