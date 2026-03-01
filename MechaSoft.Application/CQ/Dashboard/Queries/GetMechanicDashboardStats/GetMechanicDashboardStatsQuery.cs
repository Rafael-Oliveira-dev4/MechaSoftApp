using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Dashboard.Queries.GetMechanicDashboardStats;

/// <summary>
/// Query para obter estatísticas do dashboard do mecânico (ordens atribuídas, sem receita).
/// </summary>
public record GetMechanicDashboardStatsQuery(Guid MechanicId)
    : IRequest<Result<MechanicDashboardStatsResponse, Success, Error>>;

public record MechanicDashboardStatsResponse(
    int TotalAssigned,
    int Pending,
    int InProgress,
    int CompletedToday,
    List<MechanicRecentOrder> RecentOrders
);

public record MechanicRecentOrder(
    Guid Id,
    string OrderNumber,
    string VehiclePlate,
    string Description,
    string Status,
    DateTime? CreatedAt
);
