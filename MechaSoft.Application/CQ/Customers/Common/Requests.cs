namespace MechaSoft.Application.CQ.Customers.Common;

public record CreateCustomerRequest(
    string Name,
    string Email,
    string Phone,
    string? Nif,
    string Street,
    string Number,
    string Parish,
    string City,
    string PostalCode
);

public record UpdateCustomerRequest(
    string Name,
    string Email,
    string Phone,
    string? Nif,
    string Street,
    string Number,
    string Parish,
    string City,
    string PostalCode
);


