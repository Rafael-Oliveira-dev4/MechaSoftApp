using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Dashboard.Queries.GetCustomerDashboardStats;

/// <summary>
/// Query para obter estatísticas do dashboard do cliente
/// </summary>
public record GetCustomerDashboardStatsQuery(Guid CustomerId) 
    : IRequest<Result<CustomerDashboardStatsResponse, Success, Error>>;

public record CustomerDashboardStatsResponse(
    int TotalVehicles,
    int TotalOrders,
    int PendingOrders,
    int InProgressOrders,
    int CompletedOrders,
    decimal TotalSpent,
    List<RecentServiceOrder> RecentOrders
);

public record RecentServiceOrder(
    Guid Id,
    string OrderNumber,
    string VehicleBrand,
    string VehicleModel,
    string VehiclePlate,
    string Description,
    string Status,
    decimal EstimatedCost,
    DateTime? CreatedAt, // Nullable from AuditableEntity
    DateTime? EstimatedDelivery // Nullable pois pode não ter data estimada
);

