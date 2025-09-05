using FluentValidation;

namespace MechaSoft.Application.CQ.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(50).WithMessage("Model cannot exceed 50 characters.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.")
            .MaximumLength(20).WithMessage("License plate cannot exceed 20 characters.");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required.")
            .MaximumLength(30).WithMessage("Color cannot exceed 30 characters.");

        RuleFor(x => x.Year)
            .GreaterThan(1900).WithMessage("Year must be greater than 1900.")
            .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Year cannot be in the future.");

        RuleFor(x => x.VIN)
            .Length(17).WithMessage("VIN must be exactly 17 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.VIN));

        RuleFor(x => x.EngineType)
            .MaximumLength(50).WithMessage("Engine type cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.EngineType));
    }
}