using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.Customers.Common;
using MechaSoft.Domain.Core.Uow;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCustomersQuery, Result<GetCustomersResponse, Success, Error>>
{
    public async Task<Result<GetCustomersResponse, Success, Error>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all customers
            var allCustomers = await unitOfWork.CustomerRepository.GetAllAsync();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLowerInvariant();
                allCustomers = allCustomers.Where(c => 
                    c.Name.FullName.ToLowerInvariant().Contains(searchTerm) ||
                    c.Email.ToLowerInvariant().Contains(searchTerm) ||
                    c.Phone.Contains(searchTerm) ||
                    (c.Nif != null && c.Nif.Contains(searchTerm))
                );
            }

            // Calculate pagination
            var totalCount = allCustomers.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            var pagedCustomers = allCustomers
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            // Map to Response objects
            var customerResponses = pagedCustomers.Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.Name.FullName,
                Email = c.Email,
                Phone = c.Phone,
                Nif = c.Nif,
                Street = c.Address.Street,
                Number = c.Address.Number,
                Parish = c.Address.Parish,
                City = c.Address.Municipality,
                PostalCode = c.Address.PostalCode,
                Country = c.Address.District,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });

            var response = new GetCustomersResponse
            {
                Customers = customerResponses,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };

            return response;
        }
        catch (Exception)
        {
            return Error.OperationFailed;
        }
    }
}