using FluentValidation;

namespace MechaSoft.Application.CQ.Customers.Commands.CompleteCustomerProfile;

public class CompleteCustomerProfileCommandValidator : AbstractValidator<CompleteCustomerProfileCommand>
{
    public CompleteCustomerProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID é obrigatório");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Primeiro nome é obrigatório")
            .MinimumLength(2)
            .WithMessage("Primeiro nome deve ter pelo menos 2 caracteres")
            .MaximumLength(100)
            .WithMessage("Primeiro nome não pode exceder 100 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Apelido é obrigatório")
            .MinimumLength(2)
            .WithMessage("Apelido deve ter pelo menos 2 caracteres")
            .MaximumLength(100)
            .WithMessage("Apelido não pode exceder 100 caracteres")
            .When(x => x.Type == MechaSoft.Domain.Model.CustomerType.Individual); // Only required for individuals

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Telefone é obrigatório")
            .MinimumLength(9)
            .WithMessage("Telefone deve ter pelo menos 9 dígitos");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Tipo de cliente inválido");

        // Address validations
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Rua é obrigatória")
            .MaximumLength(200)
            .WithMessage("Rua não pode exceder 200 caracteres");

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Número é obrigatório")
            .MaximumLength(20)
            .WithMessage("Número não pode exceder 20 caracteres");

        RuleFor(x => x.Parish)
            .NotEmpty()
            .WithMessage("Freguesia é obrigatória")
            .MaximumLength(100)
            .WithMessage("Freguesia não pode exceder 100 caracteres");

        RuleFor(x => x.Municipality)
            .NotEmpty()
            .WithMessage("Concelho é obrigatório")
            .MaximumLength(100)
            .WithMessage("Concelho não pode exceder 100 caracteres");

        RuleFor(x => x.District)
            .NotEmpty()
            .WithMessage("Distrito é obrigatório")
            .MaximumLength(100)
            .WithMessage("Distrito não pode exceder 100 caracteres");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Código postal é obrigatório")
            .Matches(@"^\d{4}-\d{3}$")
            .WithMessage("Código postal deve estar no formato XXXX-XXX");

        RuleFor(x => x.Complement)
            .MaximumLength(100)
            .WithMessage("Complemento não pode exceder 100 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Complement));

        // Optional field validations
        RuleFor(x => x.Nif)
            .Matches(@"^\d{9}$")
            .WithMessage("NIF deve ter 9 dígitos")
            .When(x => !string.IsNullOrWhiteSpace(x.Nif));

        RuleFor(x => x.CitizenCard)
            .MaximumLength(20)
            .WithMessage("Cartão de Cidadão não pode exceder 20 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.CitizenCard));
    }
}

