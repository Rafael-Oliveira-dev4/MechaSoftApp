using FluentValidation;

namespace MechaSoft.Application.CQ.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.");

        RuleFor(x => x.Nif)
            .Length(9).WithMessage("NIF must be exactly 9 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Nif));

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street cannot exceed 100 characters.");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Number is required.")
            .MaximumLength(10).WithMessage("Number cannot exceed 10 characters.");

        RuleFor(x => x.Parish)
            .NotEmpty().WithMessage("Parish is required.")
            .MaximumLength(50).WithMessage("Parish cannot exceed 50 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .Length(8).WithMessage("Postal code must be exactly 8 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");
    }
}