namespace MechaSoft.Domain.Model;

public class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Money(decimal amount, string currency = "EUR")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = Math.Round(amount, 2); // Euros com 2 casas decimais
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add different currencies");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot subtract different currencies");

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor)
    {
        return new Money(Amount * factor, Currency);
    }

    public Money ApplyVAT(decimal vatRate = 0.23m) // IVA português padrão 23%
    {
        return new Money(Amount * (1 + vatRate), Currency);
    }

    public Money RemoveVAT(decimal vatRate = 0.23m)
    {
        return new Money(Amount / (1 + vatRate), Currency);
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Money);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public override string ToString()
    {
        return $"{Amount:F2} {Currency}";
    }

    public static bool operator ==(Money left, Money right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Money left, Money right)
    {
        return !(left == right);
    }

    public static Money operator +(Money left, Money right)
    {
        return left.Add(right);
    }

    public static Money operator -(Money left, Money right)
    {
        return left.Subtract(right);
    }

    public static Money operator *(Money money, decimal factor)
    {
        return money.Multiply(factor);
    }

    private Money()
    {
        Currency = string.Empty;
    } // Para EF Core
}