using FluentValidation;

namespace MechaSoft.Application.CQ.Inspections.Commands.CreateInspection;

public class CreateInspectionCommandValidator : AbstractValidator<CreateInspectionCommand>
{
    public CreateInspectionCommandValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle ID is required.");

        RuleFor(x => x.ServiceOrderId)
            .NotEmpty().WithMessage("Service Order ID is required.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid inspection type.");

        RuleFor(x => x.InspectionDate)
            .NotEmpty().WithMessage("Inspection date is required.")
            .GreaterThanOrEqualTo(DateTime.Today.AddMonths(-1))
            .WithMessage("Inspection date cannot be more than 1 month in the past.")
            .LessThanOrEqualTo(DateTime.Today.AddDays(30))
            .WithMessage("Inspection date cannot be more than 30 days in the future.");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.InspectionDate)
            .WithMessage("Expiry date must be after inspection date.")
            .LessThanOrEqualTo(x => x.InspectionDate.AddYears(2))
            .WithMessage("Expiry date cannot be more than 2 years after inspection date.");

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative.");

        RuleFor(x => x.InspectionCenter)
            .NotEmpty().WithMessage("Inspection center is required.")
            .MaximumLength(100).WithMessage("Inspection center name cannot exceed 100 characters.");

        RuleFor(x => x.VehicleMileage)
            .GreaterThanOrEqualTo(0).WithMessage("Vehicle mileage cannot be negative.");
    }
}

