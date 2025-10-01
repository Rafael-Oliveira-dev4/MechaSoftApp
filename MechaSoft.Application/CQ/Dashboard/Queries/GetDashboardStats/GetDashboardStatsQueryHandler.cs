using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Dashboard.Queries.GetDashboardStats;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetDashboardStatsQueryHandler> _logger;

    public GetDashboardStatsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetDashboardStatsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<DashboardStatsResponse, Success, Error>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        // Get all data in parallel for better performance
        var customersTask = _unitOfWork.CustomerRepository.GetAllAsync();
        var vehiclesTask = _unitOfWork.VehicleRepository.GetAllAsync();
        var employeesTask = _unitOfWork.EmployeeRepository.GetAllAsync();
        var serviceOrdersTask = _unitOfWork.ServiceOrderRepository.GetAllAsync();
        var partsTask = _unitOfWork.PartRepository.GetAllAsync();

        await Task.WhenAll(customersTask, vehiclesTask, employeesTask, serviceOrdersTask, partsTask);

        var customers = await customersTask;
        var vehicles = await vehiclesTask;
        var employees = await employeesTask;
        var serviceOrders = await serviceOrdersTask;
        var parts = await partsTask;

        // Calculate customer stats
        var totalCustomers = customers.Count();
        var activeCustomers = customers.Count(c => c.IsActive);

        // Calculate vehicle stats
        var totalVehicles = vehicles.Count();

        // Calculate employee stats
        var totalEmployees = employees.Count();
        var activeMechanics = employees.Count(e => e.IsActive && e.Role == EmployeeRole.Mechanic);

        // Calculate service order stats
        var today = DateTime.UtcNow.Date;
        var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        var serviceOrderStats = new ServiceOrderStats(
            Total: serviceOrders.Count(),
            Pending: serviceOrders.Count(so => so.Status == ServiceOrderStatus.Pending),
            InProgress: serviceOrders.Count(so => so.Status == ServiceOrderStatus.InProgress),
            WaitingParts: serviceOrders.Count(so => so.Status == ServiceOrderStatus.WaitingParts),
            WaitingInspection: serviceOrders.Count(so => so.Status == ServiceOrderStatus.WaitingInspection),
            CompletedToday: serviceOrders.Count(so => so.Status == ServiceOrderStatus.Completed && 
                so.UpdatedAt.HasValue && so.UpdatedAt.Value.Date == today),
            CompletedThisMonth: serviceOrders.Count(so => so.Status == ServiceOrderStatus.Completed && 
                so.UpdatedAt.HasValue && so.UpdatedAt.Value >= firstDayOfMonth)
        );

        // Calculate part stats
        var lowStockParts = parts.Where(p => p.IsLowStock() && p.IsActive).ToList();
        var criticalLowStock = lowStockParts
            .OrderBy(p => p.StockQuantity - p.MinStockLevel)
            .Take(10)
            .Select(p => new LowStockPart(
                p.Id,
                p.Code,
                p.Name,
                p.StockQuantity,
                p.MinStockLevel,
                p.MinStockLevel - p.StockQuantity
            ))
            .ToList();

        var totalInventoryValue = parts.Where(p => p.IsActive).Sum(p => p.UnitCost.Amount * p.StockQuantity);

        var partStats = new PartStats(
            TotalParts: parts.Count(p => p.IsActive),
            LowStockParts: lowStockParts.Count,
            TotalInventoryValue: totalInventoryValue,
            CriticalLowStock: criticalLowStock
        );

        // Calculate revenue
        var completedThisMonth = serviceOrders
            .Where(so => so.Status == ServiceOrderStatus.Completed && 
                   so.UpdatedAt.HasValue && so.UpdatedAt.Value >= firstDayOfMonth)
            .ToList();
        
        var completedToday = serviceOrders
            .Where(so => so.Status == ServiceOrderStatus.Completed && 
                   so.UpdatedAt.HasValue && so.UpdatedAt.Value.Date == today)
            .ToList();

        var monthRevenue = completedThisMonth.Sum(so => so.FinalCost?.Amount ?? so.EstimatedCost.Amount);
        var todayRevenue = completedToday.Sum(so => so.FinalCost?.Amount ?? so.EstimatedCost.Amount);

        var response = new DashboardStatsResponse(
            totalCustomers,
            activeCustomers,
            totalVehicles,
            totalEmployees,
            activeMechanics,
            serviceOrderStats,
            partStats,
            monthRevenue,
            todayRevenue
        );

        _logger.LogInformation("Dashboard stats generated successfully");

        return response;
    }
}

