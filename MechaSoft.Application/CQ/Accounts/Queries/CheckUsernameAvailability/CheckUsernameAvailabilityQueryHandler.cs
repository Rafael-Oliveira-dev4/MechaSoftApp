using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;

namespace MechaSoft.Application.CQ.Accounts.Queries.CheckUsernameAvailability;

public class CheckUsernameAvailabilityQueryHandler : IRequestHandler<CheckUsernameAvailabilityQuery, Result<CheckUsernameAvailabilityResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckUsernameAvailabilityQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CheckUsernameAvailabilityResponse>> Handle(
        CheckUsernameAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);
        var isAvailable = existingUser == null;

        var response = new CheckUsernameAvailabilityResponse(isAvailable);
        return Result<CheckUsernameAvailabilityResponse>.Success(response);
    }
}

public record CheckUsernameAvailabilityResponse(bool IsAvailable);

