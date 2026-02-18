using FluentValidation;

namespace MechaSoft.Application.CQ.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Invalid employee role.");

        RuleFor(x => x.HourlyRate)
            .GreaterThan(0).WithMessage("Hourly rate must be greater than 0.")
            .When(x => x.Role == Domain.Model.EmployeeRole.Mechanic);

        RuleFor(x => x.Specialties)
            .NotEmpty().WithMessage("Mechanic must have at least one specialty.")
            .When(x => x.Role == Domain.Model.EmployeeRole.Mechanic);

        RuleFor(x => x.InspectionLicenseNumber)
            .NotEmpty().WithMessage("Inspection license number is required when employee can perform inspections.")
            .When(x => x.CanPerformInspections);
    }
}

