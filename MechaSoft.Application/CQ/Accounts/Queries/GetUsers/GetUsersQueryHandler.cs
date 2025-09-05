using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<GetUsersResponse, Success, Error>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(
        IUserRepository userRepository,
        ILogger<GetUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<GetUsersResponse, Success, Error>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all users
            var allUsers = await _userRepository.GetAllAsync();

            // Apply filters
            var filteredUsers = allUsers.AsQueryable();

            if (request.Role.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.Role == request.Role.Value);
            }

            if (request.IsActive.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.IsActive == request.IsActive.Value);
            }

            // Calculate pagination
            var totalCount = filteredUsers.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            var pagedUsers = filteredUsers
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to DTOs
            var userDtos = pagedUsers.Select(u => new UserDto(
                u.Id,
                u.Username,
                u.Email,
                u.Role,
                u.IsActive,
                u.EmailConfirmed,
                u.LastLoginAt,
                u.CreatedAt ?? DateTime.UtcNow
            ));

            var response = new GetUsersResponse(
                userDtos,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users");
            return Error.OperationFailed;
        }
    }
}
