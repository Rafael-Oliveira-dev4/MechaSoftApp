using MechaSoft.Domain.Model;

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

public record CompleteCustomerProfileRequest(
    Guid UserId,
    string FirstName,
    string LastName,
    string Phone,
    CustomerType Type,
    string Street,
    string Number,
    string Parish,
    string Municipality,
    string District,
    string PostalCode,
    string? Complement,
    string? Nif,
    string? CitizenCard
);
