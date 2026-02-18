using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Commands.ForgotPassword;

public record ForgotPasswordCommand(
    string Email
) : IRequest<Result<ForgotPasswordResponse, Success, Error>>;

public record ForgotPasswordResponse(
    bool Success,
    string Message
);
