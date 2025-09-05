using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.Customers.Common;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQuery : IRequest<Result<CustomerResponse, Success, Error>>
{
    public Guid Id { get; set; }
}