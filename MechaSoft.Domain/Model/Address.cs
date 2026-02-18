namespace MechaSoft.Domain.Model;

public class Address
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Parish { get; private set; } // Freguesia
    public string Municipality { get; private set; } // Concelho
    public string District { get; private set; } // Distrito
    public string PostalCode { get; private set; } // Código Postal (XXXX-XXX)
    public string? Complement { get; private set; } // Complemento (andar, porta, etc.)

    public Address(string street, string number, string parish, string municipality,
                  string district, string postalCode, string? complement = null)
    {
        Street = ValidateRequiredField(street, nameof(street));
        Number = ValidateRequiredField(number, nameof(number));
        Parish = ValidateRequiredField(parish, nameof(parish));
        Municipality = ValidateRequiredField(municipality, nameof(municipality));
        District = ValidateRequiredField(district, nameof(district));
        PostalCode = ValidatePostalCode(postalCode);
        Complement = complement?.Trim();
    }

    private static string ValidateRequiredField(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} cannot be empty", fieldName);

        return value.Trim();
    }

    private static string ValidatePostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

        // Formato português: XXXX-XXX
        var cleanCode = postalCode.Replace("-", "").Replace(" ", "");
        if (cleanCode.Length != 7 || !cleanCode.All(char.IsDigit))
            throw new ArgumentException("Invalid Portuguese postal code format (XXXX-XXX)", nameof(postalCode));

        // Formatar corretamente
        return $"{cleanCode.Substring(0, 4)}-{cleanCode.Substring(4, 3)}";
    }

    public string FullAddress =>
        $"{Street}, {Number}" +
        (string.IsNullOrWhiteSpace(Complement) ? "" : $", {Complement}") +
        $"\n{PostalCode} {Parish}" +
        $"\n{Municipality}, {District}";

    public override string ToString()
    {
        return FullAddress;
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Street.Equals(other.Street, StringComparison.OrdinalIgnoreCase) &&
               Number.Equals(other.Number, StringComparison.OrdinalIgnoreCase) &&
               Parish.Equals(other.Parish, StringComparison.OrdinalIgnoreCase) &&
               Municipality.Equals(other.Municipality, StringComparison.OrdinalIgnoreCase) &&
               District.Equals(other.District, StringComparison.OrdinalIgnoreCase) &&
               PostalCode.Equals(other.PostalCode, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Address);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Street.ToLower(),
            Number.ToLower(),
            Parish.ToLower(),
            Municipality.ToLower(),
            District.ToLower(),
            PostalCode.ToLower()
        );
    }

    private Address()
    {
        Street = Number = Parish = Municipality = District = PostalCode = string.Empty;
    } // Para EF Core
}