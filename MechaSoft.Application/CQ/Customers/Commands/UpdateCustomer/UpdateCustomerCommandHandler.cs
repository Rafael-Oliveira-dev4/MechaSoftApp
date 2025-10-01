using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<UpdateCustomerResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCustomerCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateCustomerResponse, Success, Error>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            _logger.LogWarning("Attempt to update non-existent customer: {CustomerId}", request.Id);
            return Error.CustomerNotFound;
        }

		// Validate and parse name - ensure non-empty first name after trimming
		var trimmedName = request.Name?.Trim();
		if (string.IsNullOrWhiteSpace(trimmedName))
		{
			_logger.LogWarning("Attempt to update customer with empty name: {CustomerId}", request.Id);
			return Error.InvalidInput;
		}

		var nameParts = trimmedName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
		var firstName = nameParts[0];
		var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

        // Update properties
        customer.Name = new Name(firstName, lastName);
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.Nif = request.Nif;

        var address = new Address(
            request.Street,
            request.Number,
            request.Parish,
            request.City,
            request.City,
            request.PostalCode
        );
        customer.Address = address;

        await _unitOfWork.CustomerRepository.UpdateAsync(customer);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Customer updated successfully: {CustomerId}, {CustomerName}", customer.Id, customer.Name.FullName);

        var response = new UpdateCustomerResponse(
            customer.Id,
            customer.Name.FullName,
            customer.Email,
            customer.UpdatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

