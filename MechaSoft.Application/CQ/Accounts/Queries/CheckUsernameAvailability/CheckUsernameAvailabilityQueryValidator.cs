using FluentValidation;

namespace MechaSoft.Application.CQ.Accounts.Queries.CheckUsernameAvailability;

public class CheckUsernameAvailabilityQueryValidator : AbstractValidator<CheckUsernameAvailabilityQuery>
{
    public CheckUsernameAvailabilityQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username é obrigatório")
            .MinimumLength(3).WithMessage("Username deve ter pelo menos 3 caracteres")
            .MaximumLength(50).WithMessage("Username deve ter no máximo 50 caracteres")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username só pode conter letras, números e underscore");
    }
}

