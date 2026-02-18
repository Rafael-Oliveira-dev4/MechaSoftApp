namespace MechaSoft.Domain.Model;

public class ServiceItemRequest
{
    public Guid ServiceId { get; set; }
    public int Quantity { get; set; }
    public decimal? DiscountPercentage { get; set; }
}
