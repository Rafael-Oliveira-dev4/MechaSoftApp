using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;

namespace MechaSoft.Application.CQ.Accounts.Queries.CheckEmailAvailability;

public class CheckEmailAvailabilityQueryHandler : IRequestHandler<CheckEmailAvailabilityQuery, Result<CheckEmailAvailabilityResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckEmailAvailabilityQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CheckEmailAvailabilityResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var emailExists = await _unitOfWork.UserRepository.EmailExistsAsync(request.Email);
        
        var response = new CheckEmailAvailabilityResponse(!emailExists);
        
        return Result<CheckEmailAvailabilityResponse>.Success(response);
    }
}

