namespace MechaSoft.Domain.Model;

public class Name
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public Name(string firstName, string lastName)
    {
        FirstName = ValidateName(firstName, nameof(firstName));
        // LastName can be empty for companies
        LastName = string.IsNullOrWhiteSpace(lastName) ? string.Empty : lastName.Trim();
    }

    private static string ValidateName(string name, string paramName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{paramName} cannot be empty", paramName);

        if (name.Length < 2)
            throw new ArgumentException($"{paramName} must have at least 2 characters", paramName);

        return name.Trim();
    }

    public string FullName => string.IsNullOrWhiteSpace(LastName) 
        ? FirstName 
        : $"{FirstName} {LastName}";

    public override string ToString()
    {
        return FullName;
    }

    public bool Equals(Name? other)
    {
        if (other is null) return false;
        return FirstName.Equals(other.FirstName, StringComparison.OrdinalIgnoreCase) &&
               LastName.Equals(other.LastName, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Name);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName.ToLower(), LastName.ToLower());
    }

    private Name()
    {
        FirstName = LastName = string.Empty;
    } // Para EF Core
}