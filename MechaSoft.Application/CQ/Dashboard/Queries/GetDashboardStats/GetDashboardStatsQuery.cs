using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Dashboard.Queries.GetDashboardStats;

/// <summary>
/// Dashboard statistics query for workshop overview
/// </summary>
public record GetDashboardStatsQuery() : IRequest<Result<DashboardStatsResponse, Success, Error>>;

public record DashboardStatsResponse(
    int TotalCustomers,
    int ActiveCustomers,
    int TotalVehicles,
    int TotalEmployees,
    int ActiveMechanics,
    ServiceOrderStats ServiceOrders,
    PartStats Parts,
    decimal MonthRevenue,
    decimal TodayRevenue
);

public record ServiceOrderStats(
    int Total,
    int Pending,
    int InProgress,
    int WaitingParts,
    int WaitingInspection,
    int CompletedToday,
    int CompletedThisMonth
);

public record PartStats(
    int TotalParts,
    int LowStockParts,
    decimal TotalInventoryValue,
    List<LowStockPart> CriticalLowStock
);

public record LowStockPart(
    Guid PartId,
    string Code,
    string Name,
    int CurrentStock,
    int MinStock,
    int Deficit
);

