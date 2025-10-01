using FluentValidation;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.CreateServiceOrder;

public class CreateServiceOrderCommandValidator : AbstractValidator<CreateServiceOrderCommand>
{
    public CreateServiceOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle ID is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority value.");

        RuleFor(x => x.EstimatedDelivery)
            .GreaterThan(DateTime.UtcNow).WithMessage("Estimated delivery must be in the future.")
            .When(x => x.EstimatedDelivery.HasValue);

        RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0).WithMessage("Estimated cost cannot be negative.");

        RuleFor(x => x.InternalNotes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.InternalNotes));
    }
}

