using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.CreateServiceOrder;

public class CreateServiceOrderCommandHandler : IRequestHandler<CreateServiceOrderCommand, Result<CreateServiceOrderResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateServiceOrderCommandHandler> _logger;

    public CreateServiceOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateServiceOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreateServiceOrderResponse, Success, Error>> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        // Validate customer exists
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            _logger.LogWarning("Attempt to create service order for non-existent customer: {CustomerId}", request.CustomerId);
            return Error.CustomerNotFound;
        }

        // Validate vehicle exists
        var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
        {
            _logger.LogWarning("Attempt to create service order for non-existent vehicle: {VehicleId}", request.VehicleId);
            return Error.VehicleNotFound;
        }

        // Validate mechanic if provided
        if (request.MechanicId.HasValue)
        {
            var mechanic = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.MechanicId.Value);
            if (mechanic == null)
            {
                _logger.LogWarning("Attempt to create service order with non-existent mechanic: {MechanicId}", request.MechanicId);
                return Error.EmployeeNotFound;
            }
        }

        // Create service order
        var serviceOrder = new ServiceOrder(
            request.CustomerId,
            request.VehicleId,
            request.Description,
            request.Priority,
            new Money(request.EstimatedCost, "EUR"),
            request.EstimatedDelivery
        );

        serviceOrder.MechanicId = request.MechanicId;
        serviceOrder.RequiresInspection = request.RequiresInspection;
        serviceOrder.InternalNotes = request.InternalNotes;

        var savedOrder = await _unitOfWork.ServiceOrderRepository.SaveAsync(serviceOrder);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Service order created successfully: {OrderId}, {OrderNumber}", savedOrder.Id, savedOrder.OrderNumber);

        var response = new CreateServiceOrderResponse(
            savedOrder.Id,
            savedOrder.OrderNumber,
            savedOrder.CustomerId,
            savedOrder.VehicleId,
            savedOrder.Status,
            savedOrder.CreatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}
