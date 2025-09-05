using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileResponse, Success, Error>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<GetUserProfileQueryHandler> _logger;

    public GetUserProfileQueryHandler(
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        IEmployeeRepository employeeRepository,
        ILogger<GetUserProfileQueryHandler> logger)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task<Result<GetUserProfileResponse, Success, Error>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Error.UserNotFound;
            }

            string? customerName = null;
            string? employeeName = null;

            // Get linked customer or employee name
            if (user.CustomerId.HasValue)
            {
                var customer = await _customerRepository.GetByIdAsync(user.CustomerId.Value);
                customerName = customer?.Name.FullName;
            }
            else if (user.EmployeeId.HasValue)
            {
                var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId.Value);
                employeeName = employee?.Name.FullName;
            }

            var response = new GetUserProfileResponse(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                user.IsActive,
                user.EmailConfirmed,
                user.LastLoginAt,
                user.CustomerId,
                user.EmployeeId,
                customerName,
                employeeName
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile for user: {UserId}", request.UserId);
            return Error.OperationFailed;
        }
    }
}
