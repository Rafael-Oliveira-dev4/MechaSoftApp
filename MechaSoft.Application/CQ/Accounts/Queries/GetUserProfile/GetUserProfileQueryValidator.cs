using FluentValidation;

namespace MechaSoft.Application.CQ.Accounts.Queries.GetUserProfile;

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}
