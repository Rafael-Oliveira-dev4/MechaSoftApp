using MechaSoft.Domain.Model;
using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Commands.RegisterUser;

public record RegisterUserCommand(
    string Username,
    string Email,
    string Password,
    UserRole Role,
    Guid? CustomerId = null,
    Guid? EmployeeId = null
) : IRequest<Result<RegisterUserResponse, Success, Error>>;

public record RegisterUserResponse(
    Guid UserId,
    string Username,
    string Email,
    UserRole Role,
    bool EmailConfirmed
);
