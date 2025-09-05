using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.Customers.Common;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Queries.GetCustomers;

public class GetCustomersQuery : IRequest<Result<GetCustomersResponse, Success, Error>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}

public class GetCustomersResponse
{
    public required IEnumerable<CustomerResponse> Customers { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}