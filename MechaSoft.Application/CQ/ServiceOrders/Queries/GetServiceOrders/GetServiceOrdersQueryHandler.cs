using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Queries.GetServiceOrders;

public class GetServiceOrdersQueryHandler : IRequestHandler<GetServicesOrdersQuery, Result<GetServiceOrdersResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetServiceOrdersQueryHandler> _logger;

    public GetServiceOrdersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetServiceOrdersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetServiceOrdersResponse, Success, Error>> Handle(GetServiceOrdersQuery request, CancellationToken cancellationToken)
    {
        var allOrders = await _unitOfWork.ServiceOrderRepository.GetAllAsync();

        // Apply filters
        if (request.CustomerId.HasValue)
            allOrders = allOrders.Where(so => so.CustomerId == request.CustomerId.Value);

        if (request.Status.HasValue)
            allOrders = allOrders.Where(so => so.Status == request.Status.Value);

        // Pagination
        var totalCount = allOrders.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var pagedOrders = allOrders
            .OrderByDescending(so => so.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var orderDtos = pagedOrders.Select(so => new ServiceOrderDto(
            so.Id,
            so.OrderNumber,
            so.CustomerId,
            so.Customer?.Name.FullName ?? "Unknown",
            so.VehicleId,
            so.Vehicle?.LicensePlate ?? "Unknown",
            so.Description,
            so.Status,
            so.EstimatedCost.Amount,
            so.FinalCost?.Amount,
            so.EstimatedDelivery,
            so.CreatedAt ?? DateTime.UtcNow
        ));

        var response = new GetServiceOrdersResponse(
            orderDtos,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );

        return response;
    }
}
