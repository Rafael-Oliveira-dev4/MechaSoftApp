using FluentValidation;

namespace MechaSoft.Application.CQ.Accounts.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId é obrigatório");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username é obrigatório")
            .MinimumLength(3).WithMessage("Username deve ter pelo menos 3 caracteres")
            .MaximumLength(50).WithMessage("Username deve ter no máximo 50 caracteres")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username só pode conter letras, números e underscore");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido")
            .MaximumLength(255).WithMessage("Email deve ter no máximo 255 caracteres");
    }
}

