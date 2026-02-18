using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string? Nif,
    string Street,
    string Number,
    string Parish,
    string City,
    string PostalCode
) : IRequest<Result<UpdateCustomerResponse, Success, Error>>;

public record UpdateCustomerResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime UpdatedAt
);

