using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class ServiceOrder : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public string Description { get; set; }
    public ServiceOrderStatus Status { get; set; }
    public Priority Priority { get; set; }
    public Money EstimatedCost { get; set; }
    public Money? FinalCost { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public DateTime? ActualDelivery { get; set; }
    public Guid? MechanicId { get; set; }
    public decimal? ActualHours { get; set; } // Horas reais trabalhadas
    public bool RequiresInspection { get; set; }
    public string? InternalNotes { get; set; } // Notas internas da oficina

    // Navigation Properties
    public Customer Customer { get; set; }
    public Vehicle Vehicle { get; set; }
    public Employee? Mechanic { get; set; }
    public List<ServiceItem> Services { get; set; }
    public List<PartItem> Parts { get; set; }
    public List<Inspection> Inspections { get; set; }

    public ServiceOrder()
    {
        Id = Guid.NewGuid();
        OrderNumber = GenerateOrderNumber();
        Services = [];
        Parts = [];
        Inspections = [];
        Status = ServiceOrderStatus.Pending;
    }

    public ServiceOrder(Guid customerId, Guid vehicleId, string description,
                       Priority priority, Money estimatedCost, DateTime? estimatedDelivery = null)
    {
        Id = Guid.NewGuid();
        OrderNumber = GenerateOrderNumber();
        CustomerId = customerId;
        VehicleId = vehicleId;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Priority = priority;
        EstimatedCost = estimatedCost ?? throw new ArgumentNullException(nameof(estimatedCost));
        EstimatedDelivery = estimatedDelivery;
        Status = ServiceOrderStatus.Pending;
        Services = [];
        Parts = [];
        Inspections = [];
    }

    private static string GenerateOrderNumber()
    {
        return $"OS{DateTime.Now:yyyyMMdd}{DateTime.Now.Ticks % 10000:D4}";
    }

    public void UpdateStatus(ServiceOrderStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        if (newStatus == ServiceOrderStatus.Delivered)
        {
            ActualDelivery = DateTime.UtcNow;
        }
    }

    public Money CalculateTotalCost()
    {
        var servicesCost = Services.Sum(s => s.TotalPrice.Amount);
        var partsCost = Parts.Sum(p => p.TotalPrice.Amount);
        var inspectionsCost = Inspections.Sum(i => i.Cost.Amount);

        return new Money(servicesCost + partsCost + inspectionsCost);
    }
}
public enum ServiceOrderStatus
{
    Pending,            // Pendente
    InProgress,         // Em Progresso
    WaitingParts,       // À espera de peças
    WaitingApproval,    // À espera de aprovação do cliente
    WaitingInspection,  // À espera de inspeção
    Completed,          // Concluído
    Delivered,          // Entregue
    Cancelled           // Cancelado
}

public enum Priority
{
    Low,        // Baixa
    Medium,     // Média
    High,       // Alta
    Urgent      // Urgente                                                                                   
}




