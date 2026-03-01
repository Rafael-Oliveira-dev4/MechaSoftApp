using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Dashboard.Queries.GetMechanicDashboardStats;

public class GetMechanicDashboardStatsQueryHandler
    : IRequestHandler<GetMechanicDashboardStatsQuery, Result<MechanicDashboardStatsResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetMechanicDashboardStatsQueryHandler> _logger;

    public GetMechanicDashboardStatsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetMechanicDashboardStatsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<MechanicDashboardStatsResponse, Success, Error>> Handle(
        GetMechanicDashboardStatsQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.MechanicId);
        if (employee == null)
            return new Error("EMPLOYEE_NOT_FOUND", "Funcionário não encontrado");

        if (employee.Role != EmployeeRole.Mechanic && employee.Role != EmployeeRole.Owner)
            return new Error("INVALID_ROLE", "Apenas mecânicos podem aceder a este dashboard");

        if (!employee.IsActive)
            return new Error("INACTIVE_EMPLOYEE", "Funcionário inativo");

        var orders = (await _unitOfWork.ServiceOrderRepository.GetByMechanicIdAsync(request.MechanicId)).ToList();
        var today = DateTime.UtcNow.Date;

        var totalAssigned = orders.Count;
        var pending = orders.Count(so => so.Status == ServiceOrderStatus.Pending);
        var inProgress = orders.Count(so => so.Status == ServiceOrderStatus.InProgress);
        var completedToday = orders.Count(so =>
            so.Status == ServiceOrderStatus.Completed &&
            so.UpdatedAt.HasValue &&
            so.UpdatedAt.Value.Date == today);

        var vehicleIds = orders.Select(o => o.VehicleId).Distinct().ToList();
        var allVehicles = await _unitOfWork.VehicleRepository.GetAllAsync();
        var vehicleMap = allVehicles.Where(v => vehicleIds.Contains(v.Id)).ToDictionary(v => v.Id);

        var recentOrders = orders
            .OrderByDescending(so => so.CreatedAt)
            .Take(10)
            .Select(so =>
            {
                var vehicle = vehicleMap.GetValueOrDefault(so.VehicleId);
                return new MechanicRecentOrder(
                    so.Id,
                    so.OrderNumber,
                    vehicle?.LicensePlate ?? "N/A",
                    so.Description,
                    so.Status.ToString(),
                    so.CreatedAt
                );
            })
            .ToList();

        var response = new MechanicDashboardStatsResponse(
            totalAssigned,
            pending,
            inProgress,
            completedToday,
            recentOrders
        );

        _logger.LogInformation(
            "Mechanic dashboard stats generated for MechanicId: {MechanicId} - {TotalAssigned} orders assigned",
            request.MechanicId, totalAssigned);

        return response;
    }
}
