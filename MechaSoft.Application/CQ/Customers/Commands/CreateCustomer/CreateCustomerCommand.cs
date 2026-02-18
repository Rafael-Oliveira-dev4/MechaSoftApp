using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<Result<CreateCustomerResponse, Success, Error>>
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public string? Nif { get; init; }
    public required string Street { get; init; }
    public required string Number { get; init; }
    public required string Parish { get; init; }
    public required string City { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }
}

public class CreateCustomerResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Nif { get; set; }
    public DateTime CreatedAt { get; set; }
}