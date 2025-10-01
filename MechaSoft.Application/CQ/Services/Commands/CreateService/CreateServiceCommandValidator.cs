using FluentValidation;

namespace MechaSoft.Application.CQ.Services.Commands.CreateService;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Service name is required.")
            .MaximumLength(100).WithMessage("Service name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid service category.");

        RuleFor(x => x.EstimatedHours)
            .GreaterThan(0).WithMessage("Estimated hours must be greater than 0.");

        RuleFor(x => x.PricePerHour)
            .GreaterThan(0).WithMessage("Price per hour must be greater than 0.");

        RuleFor(x => x.FixedPrice)
            .GreaterThan(0).WithMessage("Fixed price must be greater than 0.")
            .When(x => x.FixedPrice.HasValue);
    }
}

