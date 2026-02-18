namespace MechaSoft.Domain.Events;

public class LowStockDetectedEvent : DomainEvent
{
    public Guid PartId { get; private set; }
    public string PartName { get; private set; }
    public int CurrentStock { get; private set; }
    public int MinStockLevel { get; private set; }

    public LowStockDetectedEvent(Guid partId, string partName, int currentStock, int minStockLevel)
    {
        PartId = partId;
        PartName = partName;
        CurrentStock = currentStock;
        MinStockLevel = minStockLevel;
    }
}