namespace MechaSoft.Domain.Model;

public class ScheduleInspectionRequest
{
    public Guid VehicleId { get; set; }
    public Guid ServiceOrderId { get; set; }
    public InspectionType Type { get; set; }
    public DateTime InspectionDate { get; set; }
    public string InspectionCenter { get; set; } = null!;
    public decimal Cost { get; set; }
    public int VehicleMileage { get; set; }
}