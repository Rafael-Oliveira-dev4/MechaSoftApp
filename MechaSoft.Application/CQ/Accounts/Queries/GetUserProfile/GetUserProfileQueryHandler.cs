using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserProfileQueryHandler> _logger;

    public GetUserProfileQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUserProfileQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetUserProfileResponse, Success, Error>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        // Get user
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Error.UserNotFound;

        string? customerName = null;
        string? employeeName = null;

        // Get linked customer or employee name
        if (user.CustomerId.HasValue)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(user.CustomerId.Value);
            customerName = customer?.Name.FullName;
        }
        else if (user.EmployeeId.HasValue)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(user.EmployeeId.Value);
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
}
