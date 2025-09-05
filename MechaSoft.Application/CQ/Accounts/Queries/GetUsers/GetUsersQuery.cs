using MechaSoft.Domain.Model;
using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Queries.GetUsers;

public record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    UserRole? Role = null,
    bool? IsActive = null
) : IRequest<Result<GetUsersResponse, Success, Error>>;

public record GetUsersResponse(
    IEnumerable<UserDto> Users,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    UserRole Role,
    bool IsActive,
    bool EmailConfirmed,
    DateTime? LastLoginAt,
    DateTime CreatedAt
);
