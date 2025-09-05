using MechaSoft.Domain.Model;
using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Queries.GetUserProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<Result<GetUserProfileResponse, Success, Error>>;

public record GetUserProfileResponse(
    Guid UserId,
    string Username,
    string Email,
    UserRole Role,
    bool IsActive,
    bool EmailConfirmed,
    DateTime? LastLoginAt,
    Guid? CustomerId,
    Guid? EmployeeId,
    string? CustomerName,
    string? EmployeeName
);
