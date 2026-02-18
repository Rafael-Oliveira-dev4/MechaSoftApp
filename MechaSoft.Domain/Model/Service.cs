using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class Service : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ServiceCategory Category { get; set; }
    public decimal EstimatedHours { get; set; } // Tempo estimado em horas
    public Money PricePerHour { get; set; }
    public Money? FixedPrice { get; set; } // Preço fixo (alternativa ao preço por hora)
    public bool IsActive { get; set; }
    public bool RequiresInspection { get; set; } // Se requer inspeção após o serviço

    public Service()
    {
        Id = Guid.NewGuid();
        Name = Description = string.Empty;
        PricePerHour = new Money(0);
        IsActive = true;
    }

    public Service(string name, string description, ServiceCategory category,
                   decimal estimatedHours, Money pricePerHour, Money? fixedPrice = null)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Category = category;
        EstimatedHours = estimatedHours;
        PricePerHour = pricePerHour ?? throw new ArgumentNullException(nameof(pricePerHour));
        FixedPrice = fixedPrice;
        IsActive = true;
    }

    public Money CalculateServiceCost(decimal actualHours)
    {
        if (FixedPrice is { } fixedPrice)
            return fixedPrice;

        return PricePerHour.Multiply(actualHours);
    }
}
public enum ServiceCategory
{
    Engine,             // Motor
    Transmission,       // Transmissão
    Brakes,            // Travões
    Suspension,        // Suspensão
    Electrical,        // Elétrico
    AirConditioning,   // Ar Condicionado
    Bodywork,          // Chapa/Pintura
    Maintenance,       // Manutenção
    Diagnostic,        // Diagnóstico
    Inspection,        // Inspeção
    Tires,            // Pneus
    Exhaust,          // Escape
    Cooling,          // Arrefecimento
    Fuel              // Combustível
}