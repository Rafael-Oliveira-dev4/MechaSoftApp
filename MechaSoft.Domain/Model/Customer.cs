using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class Customer : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public required Name Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Nif { get; set; } // Número de Identificação Fiscal
    public string? CitizenCard { get; set; } // Cartão de Cidadão
    public required Address Address { get; set; }
    public CustomerType Type { get; set; }
    public string? Notes { get; set; } // Notas sobre o cliente

    // Navigation Properties
    public List<Vehicle> Vehicles { get; set; }
    public List<ServiceOrder> ServiceOrders { get; set; }

    public Customer()
    {
        Id = Guid.NewGuid();
        Vehicles = [];
        ServiceOrders = [];
    }

    public Customer(string firstName, string lastName, string email, string phone,
                   Address address, CustomerType type, string? nif = null, string? citizenCard = null)
    {
        Id = Guid.NewGuid();
        Name = new Name(firstName, lastName);
        Email = ValidateEmail(email);
        Phone = ValidatePhone(phone);
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Type = type;
        Nif = ValidateNif(nif);
        CitizenCard = citizenCard;
        Vehicles = [];
        ServiceOrders = [];
    }

    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format", nameof(email));

        return email.ToLower().Trim();
    }

    private static string ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty", nameof(phone));

        // Remove espaços e caracteres especiais
        var cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

        // Validação básica para números portugueses (9 dígitos)
        if (cleanPhone.Length < 9)
            throw new ArgumentException("Invalid Portuguese phone number", nameof(phone));

        return phone;
    }

    private static string? ValidateNif(string? nif)
    {
        if (string.IsNullOrWhiteSpace(nif))
            return null;

        // NIF português tem 9 dígitos
        var cleanNif = nif.Replace(" ", "");
        if (cleanNif.Length != 9 || !cleanNif.All(char.IsDigit))
            throw new ArgumentException("Invalid Portuguese NIF format", nameof(nif));

        return cleanNif;
    }

    public void UpdateContactInfo(string email, string phone)
    {
        Email = ValidateEmail(email);
        Phone = ValidatePhone(phone);
        UpdateTimestamp();
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        UpdateTimestamp();
    }

    public void AddNotes(string notes)
    {
        if (string.IsNullOrWhiteSpace(Notes))
            Notes = notes;
        else
            Notes += Environment.NewLine + notes;

        UpdateTimestamp();
    }

    public bool HasValidNif()
    {
        return !string.IsNullOrWhiteSpace(Nif);
    }

    public bool IsCompany()
    {
        return Type == CustomerType.Company;
    }

    public bool IsIndividual()
    {
        return Type == CustomerType.Individual;
    }
}
public enum CustomerType
{
    Individual, // Particular
    Company     // Empresa
}