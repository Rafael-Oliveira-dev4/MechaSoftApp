using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.Customers.Common;
using MechaSoft.Domain.Core.Uow;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCustomerByIdQuery, Result<CustomerResponse, Success, Error>>
{
    public async Task<Result<CustomerResponse, Success, Error>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await unitOfWork.CustomerRepository.GetByIdAsync(request.Id);
            if (customer == null)
            {
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
        catch (Exception)
        {
            return Error.OperationFailed;
        }
    }
}