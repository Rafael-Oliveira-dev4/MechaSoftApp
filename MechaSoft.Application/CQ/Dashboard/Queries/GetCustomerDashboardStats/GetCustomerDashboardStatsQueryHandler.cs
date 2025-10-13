using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Dashboard.Queries.GetCustomerDashboardStats;

public class GetCustomerDashboardStatsQueryHandler 
    : IRequestHandler<GetCustomerDashboardStatsQuery, Result<CustomerDashboardStatsResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCustomerDashboardStatsQueryHandler> _logger;

    public GetCustomerDashboardStatsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCustomerDashboardStatsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CustomerDashboardStatsResponse, Success, Error>> Handle(
        GetCustomerDashboardStatsQuery request,
        CancellationToken cancellationToken)
    {
        // Verify customer exists
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
            return new Error("CUSTOMER_NOT_FOUND", "Cliente não encontrado");

        // Get all customer data sequentially (DbContext thread-safety)
        var allVehicles = await _unitOfWork.VehicleRepository.GetAllAsync();
        var allServiceOrders = await _unitOfWork.ServiceOrderRepository.GetAllAsync();

        // Filter by customer
        var vehicles = allVehicles.Where(v => v.CustomerId == request.CustomerId).ToList();
        var serviceOrders = allServiceOrders.Where(so => so.CustomerId == request.CustomerId).ToList();

        // Calculate stats
        var totalVehicles = vehicles.Count;
        var totalOrders = serviceOrders.Count;
        var pendingOrders = serviceOrders.Count(so => so.Status == ServiceOrderStatus.Pending);
        var inProgressOrders = serviceOrders.Count(so => so.Status == ServiceOrderStatus.InProgress);
        var completedOrders = serviceOrders.Count(so => so.Status == ServiceOrderStatus.Completed);

        // Calculate total spent (only completed orders)
        var totalSpent = serviceOrders
            .Where(so => so.Status == ServiceOrderStatus.Completed)
            .Sum(so => so.FinalCost?.Amount ?? so.EstimatedCost?.Amount ?? 0);

        // Get recent orders (last 5)
        var recentOrders = serviceOrders
            .OrderByDescending(so => so.CreatedAt)
            .Take(5)
            .Select(so =>
            {
                var vehicle = vehicles.FirstOrDefault(v => v.Id == so.VehicleId);
                return new RecentServiceOrder(
                    so.Id,
                    so.OrderNumber,
                    vehicle?.Brand ?? "N/A",
                    vehicle?.Model ?? "N/A",
                    vehicle?.LicensePlate ?? "N/A",
                    so.Description,
                    so.Status.ToString(),
                    so.EstimatedCost?.Amount ?? 0,
                    so.CreatedAt,
                    so.EstimatedDelivery
                );
            })
            .ToList();

        var response = new CustomerDashboardStatsResponse(
            totalVehicles,
            totalOrders,
            pendingOrders,
            inProgressOrders,
            completedOrders,
            totalSpent,
            recentOrders
        );

        _logger.LogInformation(
            "Customer dashboard stats generated for CustomerId: {CustomerId} - {TotalOrders} orders, {TotalVehicles} vehicles",
            request.CustomerId, totalOrders, totalVehicles);

        return response;
    }
}

