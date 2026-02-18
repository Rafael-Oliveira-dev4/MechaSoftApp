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

        // Map ServiceItems to DTOs
        var serviceDtos = new List<ServiceItemDto>();
        foreach (var serviceItem in serviceOrder.Services)
        {
            var service = serviceItem.Service ?? await _unitOfWork.ServiceRepository.GetByIdAsync(serviceItem.ServiceId);
            var serviceMechanic = serviceItem.MechanicId.HasValue
                ? serviceItem.Mechanic ?? await _unitOfWork.EmployeeRepository.GetByIdAsync(serviceItem.MechanicId.Value)
                : null;

            serviceDtos.Add(new ServiceItemDto(
                serviceItem.Id,
                serviceItem.ServiceId,
                service?.Name ?? "Unknown",
                serviceItem.Quantity,
                serviceItem.EstimatedHours,
                serviceItem.UnitPrice.Amount,
                serviceItem.DiscountPercentage,
                serviceItem.TotalPrice.Amount,
                serviceItem.Status.ToString(),
                serviceItem.MechanicId,
                serviceMechanic?.Name.FullName
            ));
        }

        // Map PartItems to DTOs (PartItem usa chave composta, não tem Id único)
        var partDtos = new List<PartItemDto>();
        if (serviceOrder.Parts != null && serviceOrder.Parts.Count > 0)
        {
            foreach (var partItem in serviceOrder.Parts)
            {
                var part = partItem.Part ?? await _unitOfWork.PartRepository.GetByIdAsync(partItem.PartId);

                partDtos.Add(new PartItemDto(
                    partItem.PartId,
                    part?.Name ?? "Unknown",
                    part?.Code ?? "N/A",
                    partItem.Quantity,
                    partItem.UnitPrice.Amount,
                    partItem.DiscountPercentage,
                    partItem.TotalPrice.Amount
                ));
            }
        }

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
            serviceOrder.UpdatedAt,
            serviceDtos,
            partDtos
        );

        return response;
    }
}
