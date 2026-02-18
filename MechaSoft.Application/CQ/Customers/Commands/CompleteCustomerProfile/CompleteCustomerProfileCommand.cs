using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Commands.CompleteCustomerProfile;

public class CompleteCustomerProfileCommand : IRequest<Result<CompleteCustomerProfileResponse, Success, Error>>
{
    public required Guid UserId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Phone { get; init; }
    public required CustomerType Type { get; init; }
    
    // Address fields
    public required string Street { get; init; }
    public required string Number { get; init; }
    public required string Parish { get; init; }
    public required string Municipality { get; init; }
    public required string District { get; init; }
    public required string PostalCode { get; init; }
    public string? Complement { get; init; }
    
    // Optional fields
    public string? Nif { get; init; }
    public string? CitizenCard { get; init; }
}

public class CompleteCustomerProfileResponse
{
    public Guid CustomerId { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public CustomerType Type { get; set; }
    public string? Nif { get; set; }
    public DateTime CreatedAt { get; set; }
}

