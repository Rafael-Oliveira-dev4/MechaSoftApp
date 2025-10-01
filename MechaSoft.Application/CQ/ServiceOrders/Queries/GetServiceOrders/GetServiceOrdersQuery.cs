using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Queries.GetServiceOrders;

public record GetServiceOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? CustomerId = null,
    ServiceOrderStatus? Status = null
) : IRequest<Result<GetServiceOrdersResponse, Success, Error>>;

public record GetServiceOrdersResponse(
    IEnumerable<ServiceOrderDto> ServiceOrders,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public record ServiceOrderDto(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    string CustomerName,
    Guid VehicleId,
    string VehicleLicensePlate,
    string Description,
    ServiceOrderStatus Status,
    decimal EstimatedCost,
    decimal? FinalCost,
    DateTime? EstimatedDelivery,
    DateTime CreatedAt
);

