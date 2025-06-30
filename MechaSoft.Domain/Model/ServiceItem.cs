using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class ServiceItem : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; } // ← Propriedade necessária para IEntity<Guid>
    public Guid ServiceOrderId { get; set; }
    public Guid ServiceId { get; set; }
    public int Quantity { get; set; }
    public decimal EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; } // Horas realmente trabalhadas
    public Money UnitPrice { get; set; } // Preço por hora ou preço fixo
    public decimal? DiscountPercentage { get; set; }
    public Money TotalPrice { get; set; }
    public ServiceItemStatus Status { get; set; }
    public string? Notes { get; set; } // Notas específicas deste serviço
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? MechanicId { get; set; } // Mecânico responsável por este serviço específico

    // Navigation Properties
    public ServiceOrder ServiceOrder { get; set; }
    public Service Service { get; set; }
    public Employee? Mechanic { get; set; }

    public ServiceItem()
    {
        Id = Guid.NewGuid(); // ← Inicializar o Id
        Status = ServiceItemStatus.Pending;
    }

    public ServiceItem(Guid serviceOrderId, Guid serviceId, int quantity,
                      decimal estimatedHours, Money unitPrice, decimal? discountPercentage = null)
    {
        Id = Guid.NewGuid(); // ← Inicializar o Id
        ServiceOrderId = serviceOrderId;
        ServiceId = serviceId;
        Quantity = quantity;
        EstimatedHours = estimatedHours;
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
        DiscountPercentage = discountPercentage;
        Status = ServiceItemStatus.Pending;
        CalculateTotalPrice();
    }

    // ... resto dos métodos permanecem iguais
    public void UpdateActualHours(decimal actualHours)
    {
        if (actualHours < 0)
            throw new ArgumentException("Actual hours cannot be negative", nameof(actualHours));
        ActualHours = actualHours;
        RecalculateTotalPrice();
    }

    public void StartService(Guid? mechanicId = null)
    {
        if (Status != ServiceItemStatus.Pending)
            throw new InvalidOperationException($"Cannot start service in status {Status}");
        Status = ServiceItemStatus.InProgress;
        StartedAt = DateTime.UtcNow;
        MechanicId = mechanicId;
    }

    public void CompleteService(decimal actualHours, string? notes = null)
    {
        if (Status != ServiceItemStatus.InProgress)
            throw new InvalidOperationException($"Cannot complete service in status {Status}");
        ActualHours = actualHours;
        Status = ServiceItemStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        Notes = notes;
        RecalculateTotalPrice();
    }

    public void PauseService(string? reason = null)
    {
        if (Status != ServiceItemStatus.InProgress)
            throw new InvalidOperationException($"Cannot pause service in status {Status}");
        Status = ServiceItemStatus.Paused;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? reason : $"{Notes}\n{reason}";
        }
    }

    public void ResumeService()
    {
        if (Status != ServiceItemStatus.Paused)
            throw new InvalidOperationException($"Cannot resume service in status {Status}");
        Status = ServiceItemStatus.InProgress;
    }

    public void CancelService(string? reason = null)
    {
        if (Status == ServiceItemStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed service");
        Status = ServiceItemStatus.Cancelled;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? reason : $"{Notes}\n{reason}";
        }
    }

    private void CalculateTotalPrice()
    {
        var hoursToUse = ActualHours ?? EstimatedHours;
        var subtotal = UnitPrice.Multiply(hoursToUse * Quantity);
        if (DiscountPercentage.HasValue && DiscountPercentage > 0)
        {
            var discount = subtotal.Multiply(DiscountPercentage.Value / 100);
            TotalPrice = subtotal.Subtract(discount);
        }
        else
        {
            TotalPrice = subtotal;
        }
    }

    private void RecalculateTotalPrice()
    {
        CalculateTotalPrice();
    }

    public decimal GetHoursVariance()
    {
        if (!ActualHours.HasValue)
            return 0;
        return ActualHours.Value - EstimatedHours;
    }

    public bool IsOverEstimate()
    {
        return GetHoursVariance() > 0;
    }

    public bool IsUnderEstimate()
    {
        return GetHoursVariance() < 0;
    }

    public TimeSpan? GetServiceDuration()
    {
        if (!StartedAt.HasValue)
            return null;
        var endTime = CompletedAt ?? DateTime.UtcNow;
        return endTime - StartedAt.Value;
    }
}
