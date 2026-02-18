using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class Inspection : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid ServiceOrderId { get; set; }
    public InspectionType Type { get; set; }
    public DateTime InspectionDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public InspectionResult Result { get; set; }
    public string? Observations { get; set; }
    public Money Cost { get; set; }
    public string? CertificateNumber { get; set; }
    public string InspectionCenter { get; set; }
    public int VehicleMileage { get; set; } // Quilometragem no momento da inspeção

    // Navigation Properties
    public Vehicle Vehicle { get; set; }
    public ServiceOrder ServiceOrder { get; set; }

    public Inspection()
    {
        Id = Guid.NewGuid();
        Cost = new Money(0);
        InspectionCenter = string.Empty;
        Vehicle = null!;
        ServiceOrder = null!;
    }

    public Inspection(Guid vehicleId, Guid serviceOrderId, InspectionType type,
                     DateTime inspectionDate, DateTime expiryDate, Money cost,
                     string inspectionCenter, int vehicleMileage)
    {
        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        ServiceOrderId = serviceOrderId;
        Type = type;
        InspectionDate = inspectionDate;
        ExpiryDate = expiryDate;
        Cost = cost ?? throw new ArgumentNullException(nameof(cost));
        InspectionCenter = inspectionCenter ?? throw new ArgumentNullException(nameof(inspectionCenter));
        VehicleMileage = vehicleMileage;
        Result = InspectionResult.Pending;
        Vehicle = null!;
        ServiceOrder = null!;
    }

    public void UpdateResult(InspectionResult result, string? observations = null, string? certificateNumber = null)
    {
        Result = result;
        Observations = observations;
        CertificateNumber = certificateNumber;
        UpdatedAt = DateTime.UtcNow;
    }
}
public enum InspectionType
{
    Periodic,       // Inspeção Periódica
    Extraordinary,  // Inspeção Extraordinária
    Recheck        // Reinspeção
}

public enum InspectionResult
{
    Pending,    // Pendente
    Approved,   // Aprovado
    Rejected,   // Reprovado
    Conditional // Condicional
}
