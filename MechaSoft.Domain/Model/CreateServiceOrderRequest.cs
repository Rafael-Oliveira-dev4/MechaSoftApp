namespace MechaSoft.Domain.Model;

public class CreateServiceOrderRequest
{
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public List<ServiceItemRequest> Services { get; set; } = [];
    public List<PartItemRequest> Parts { get; set; } = [];
    public Guid? MechanicId { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public decimal EstimatedCost { get; set; }
    public bool RequiresInspection { get; set; }
    public string? InternalNotes { get; set; }
}
