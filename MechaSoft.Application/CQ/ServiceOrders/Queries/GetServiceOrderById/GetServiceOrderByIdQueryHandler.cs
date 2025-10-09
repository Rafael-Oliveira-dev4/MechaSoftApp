using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Queries.GetServiceOrderById;

public class GetServiceOrderByIdQueryHandler : IRequestHandler<GetServiceOrderByIdQuery, Result<ServiceOrderResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetServiceOrderByIdQueryHandler> _logger;

    public GetServiceOrderByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetServiceOrderByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ServiceOrderResponse, Success, Error>> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.Id);
        if (serviceOrder == null)
        {
            _logger.LogWarning("Service order not found: {ServiceOrderId}", request.Id);
            return Error.ServiceOrderNotFound;
        }

        // Get related entities
        var customer = serviceOrder.Customer ?? await _unitOfWork.CustomerRepository.GetByIdAsync(serviceOrder.CustomerId);
        var vehicle = serviceOrder.Vehicle ?? await _unitOfWork.VehicleRepository.GetByIdAsync(serviceOrder.VehicleId);
        var mechanic = serviceOrder.MechanicId.HasValue 
            ? serviceOrder.Mechanic ?? await _unitOfWork.EmployeeRepository.GetByIdAsync(serviceOrder.MechanicId.Value)
            : null;

        var response = new ServiceOrderResponse(
            serviceOrder.Id,
            serviceOrder.OrderNumber,
            serviceOrder.CustomerId,
            customer?.Name.FullName ?? "Unknown",
            serviceOrder.VehicleId,
            vehicle != null ? $"{vehicle.Brand} {vehicle.Model} - {vehicle.LicensePlate}" : "Unknown",
            serviceOrder.Description,
            serviceOrder.Status,
            serviceOrder.Priority,
            serviceOrder.EstimatedCost.Amount,
            serviceOrder.FinalCost?.Amount,
            serviceOrder.EstimatedDelivery,
            serviceOrder.ActualDelivery,
            serviceOrder.MechanicId,
            mechanic?.Name.FullName,
            serviceOrder.ActualHours,
            serviceOrder.RequiresInspection,
            serviceOrder.InternalNotes,
            serviceOrder.CreatedAt ?? DateTime.UtcNow,
            serviceOrder.UpdatedAt
        );

        return response;
    }
}
