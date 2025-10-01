using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.Customers.Common;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

    public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCustomerByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CustomerResponse, Success, Error>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            _logger.LogWarning("Customer not found: {CustomerId}", request.Id);
            return Error.CustomerNotFound;
        }

        var customerResponse = new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            Nif = customer.Nif,
            Street = customer.Address.Street,
            Number = customer.Address.Number,
            Parish = customer.Address.Parish,
            City = customer.Address.Municipality,
            PostalCode = customer.Address.PostalCode,
            Country = customer.Address.District,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };

        return customerResponse;
    }
}