using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCustomerCommand, Result<CreateCustomerResponse, Success, Error>>
{
    public async Task<Result<CreateCustomerResponse, Success, Error>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if customer with same NIF already exists (if NIF provided)
            if (!string.IsNullOrWhiteSpace(request.Nif))
            {
                var existingCustomerByNif = await unitOfWork.CustomerRepository.GetByNifAsync(request.Nif);
                if (existingCustomerByNif != null)
                {
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

            // Create customer
            var customer = new Customer
            {
                Name = new Name(
                    request.Name.Split(' ')[0], // First name
                    string.Join(" ", request.Name.Split(' ').Skip(1)) // Last name
                ),
                Email = request.Email,
                Phone = request.Phone,
                Address = address,
                Type = CustomerType.Individual, // Default to Individual
                Nif = request.Nif
            };

            // Save customer
            var savedCustomer = await unitOfWork.CustomerRepository.SaveAsync(customer);
            await unitOfWork.CommitAsync(cancellationToken);

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
        catch (Exception)
        {
            return Error.OperationFailed;
        }
    }
}