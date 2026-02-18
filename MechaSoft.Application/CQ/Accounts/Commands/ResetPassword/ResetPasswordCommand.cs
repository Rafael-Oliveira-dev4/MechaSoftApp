using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string ResetToken,
    string NewPassword,
    string ConfirmNewPassword
) : IRequest<Result<ResetPasswordResponse, Success, Error>>;

public record ResetPasswordResponse(
    bool Success,
    string Message
);
