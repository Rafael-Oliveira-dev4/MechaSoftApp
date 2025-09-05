namespace MechaSoft.Application.CQ.Customers.Common;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Nif { get; set; }
    public required string Street { get; set; }
    public required string Number { get; set; }
    public required string Parish { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
