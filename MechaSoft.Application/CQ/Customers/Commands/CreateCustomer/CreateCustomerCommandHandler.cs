using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CreateCustomerResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCustomerCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreateCustomerResponse, Success, Error>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Check if customer with same NIF already exists (if NIF provided)
        if (!string.IsNullOrWhiteSpace(request.Nif))
        {
            var existingCustomerByNif = await _unitOfWork.CustomerRepository.GetByNifAsync(request.Nif);
            if (existingCustomerByNif != null)
            {
                _logger.LogWarning("Attempt to create customer with existing NIF: {Nif}", request.Nif);
                return Error.ExistingCustomer;
            }
        }

        // Create address
        var address = new Address(
            request.Street,
            request.Number,
            request.Parish,
            request.City, // Municipality
            request.City, // District (using city as district for simplicity)
            request.PostalCode
        );

        // Parse name - split by first space
        var nameParts = request.Name.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        var firstName = nameParts[0];
        var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

        // Create customer
        var customer = new Customer
        {
            Name = new Name(firstName, lastName),
            Email = request.Email,
            Phone = request.Phone,
            Address = address,
            Type = CustomerType.Individual, // Default to Individual
            Nif = request.Nif
        };

        // Save customer
        var savedCustomer = await _unitOfWork.CustomerRepository.SaveAsync(customer);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Customer created successfully: {CustomerId}, {CustomerName}", savedCustomer.Id, savedCustomer.Name.FullName);

        var response = new CreateCustomerResponse
        {
            Id = savedCustomer.Id,
            Name = savedCustomer.Name.FullName,
            Email = savedCustomer.Email,
            Phone = savedCustomer.Phone,
            Nif = savedCustomer.Nif,
            CreatedAt = DateTime.UtcNow
        };

        return response;
    }
}