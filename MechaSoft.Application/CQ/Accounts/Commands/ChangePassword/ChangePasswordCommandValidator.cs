using FluentValidation;

namespace MechaSoft.Application.CQ.Accounts.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("New password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character");

        RuleFor(x => x)
            .Must(x => x.CurrentPassword != x.NewPassword)
            .WithMessage("New password must be different from current password");
    }
}
