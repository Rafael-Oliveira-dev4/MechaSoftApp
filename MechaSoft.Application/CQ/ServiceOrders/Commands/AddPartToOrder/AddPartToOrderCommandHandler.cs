using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.AddPartToOrder;

public class AddPartToOrderCommandHandler : IRequestHandler<AddPartToOrderCommand, Result<AddPartToOrderResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPartToOrderCommandHandler> _logger;

    public AddPartToOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<AddPartToOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<AddPartToOrderResponse, Success, Error>> Handle(AddPartToOrderCommand request, CancellationToken cancellationToken)
    {
        var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (serviceOrder == null)
        {
            _logger.LogWarning("Attempt to add part to non-existent order: {ServiceOrderId}", request.ServiceOrderId);
            return Error.ServiceOrderNotFound;
        }

        var part = await _unitOfWork.PartRepository.GetByIdAsync(request.PartId);
        if (part == null)
        {
            _logger.LogWarning("Attempt to add non-existent part: {PartId}", request.PartId);
            return Error.PartNotFound;
        }

        // Check stock availability
        if (part.StockQuantity < request.Quantity)
        {
            _logger.LogWarning("Insufficient stock for part: {PartCode}, Requested: {Requested}, Available: {Available}", 
                part.Code, request.Quantity, part.StockQuantity);
            return Error.InsufficientStock;
        }

        // Create part item
        var partItem = new PartItem(
            request.ServiceOrderId,
            request.PartId,
            request.Quantity,
            part.SalePrice,
            request.DiscountPercentage
        );

        // Add to service order
        serviceOrder.Parts.Add(partItem);

        // Update stock (remove from inventory) - won't throw because we validated
        part.UpdateStock(request.Quantity, StockMovementType.Out);

        // Recalculate estimated cost
        serviceOrder.EstimatedCost = serviceOrder.CalculateTotalCost();

        await _unitOfWork.ServiceOrderRepository.UpdateAsync(serviceOrder);
        await _unitOfWork.PartRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Part added to order: {OrderNumber}, Part: {PartCode}, Quantity: {Quantity}", 
            serviceOrder.OrderNumber, part.Code, request.Quantity);

        if (part.IsLowStock())
        {
            _logger.LogWarning("Low stock alert after order: {PartCode}, Current stock: {StockQuantity}", 
                part.Code, part.StockQuantity);
        }

        var response = new AddPartToOrderResponse(
            serviceOrder.Id,
            part.Id,
            part.Name,
            request.Quantity,
            partItem.TotalPrice.Amount,
            part.StockQuantity
        );

        return response;
    }
}

