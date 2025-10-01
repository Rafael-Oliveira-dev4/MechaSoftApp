using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.AddServiceToOrder;

public class AddServiceToOrderCommandHandler : IRequestHandler<AddServiceToOrderCommand, Result<AddServiceToOrderResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddServiceToOrderCommandHandler> _logger;

    public AddServiceToOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<AddServiceToOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<AddServiceToOrderResponse, Success, Error>> Handle(AddServiceToOrderCommand request, CancellationToken cancellationToken)
    {
        var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (serviceOrder == null)
        {
            _logger.LogWarning("Attempt to add service to non-existent order: {ServiceOrderId}", request.ServiceOrderId);
            return Error.ServiceOrderNotFound;
        }

        var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.ServiceId);
        if (service == null)
        {
            _logger.LogWarning("Attempt to add non-existent service: {ServiceId}", request.ServiceId);
            return Error.ServiceNotFound;
        }

        // Determine unit price (fixed or per hour)
        var unitPrice = service.FixedPrice ?? service.PricePerHour;

        // Create service item
        var serviceItem = new ServiceItem(
            request.ServiceOrderId,
            request.ServiceId,
            request.Quantity,
            request.EstimatedHours,
            unitPrice,
            request.DiscountPercentage
        );

        serviceItem.MechanicId = request.MechanicId;

        // Add to service order
        serviceOrder.Services.Add(serviceItem);

        // Recalculate estimated cost
        serviceOrder.EstimatedCost = serviceOrder.CalculateTotalCost();

        await _unitOfWork.ServiceOrderRepository.UpdateAsync(serviceOrder);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Service added to order: {OrderNumber}, Service: {ServiceName}", 
            serviceOrder.OrderNumber, service.Name);

        var response = new AddServiceToOrderResponse(
            serviceOrder.Id,
            serviceItem.Id,
            service.Name,
            serviceItem.TotalPrice.Amount
        );

        return response;
    }
}
