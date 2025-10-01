using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.CancelServiceOrder;

public class CancelServiceOrderCommandHandler : IRequestHandler<CancelServiceOrderCommand, Result<CancelServiceOrderResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelServiceOrderCommandHandler> _logger;

    public CancelServiceOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelServiceOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CancelServiceOrderResponse, Success, Error>> Handle(CancelServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (serviceOrder == null)
        {
            _logger.LogWarning("Attempt to cancel non-existent service order: {ServiceOrderId}", request.ServiceOrderId);
            return Error.ServiceOrderNotFound;
        }

        // Can't cancel if already delivered or cancelled
        if (serviceOrder.Status == ServiceOrderStatus.Delivered)
        {
            _logger.LogWarning("Attempt to cancel already delivered service order: {OrderNumber}", serviceOrder.OrderNumber);
            return new Error("CannotCancelDelivered", "Cannot cancel a service order that has already been delivered");
        }

        if (serviceOrder.Status == ServiceOrderStatus.Cancelled)
        {
            _logger.LogWarning("Attempt to cancel already cancelled service order: {OrderNumber}", serviceOrder.OrderNumber);
            return new Error("AlreadyCancelled", "Service order is already cancelled");
        }

        // Return parts to stock
        int partsReturned = 0;
        foreach (var partItem in serviceOrder.Parts)
        {
            var part = await _unitOfWork.PartRepository.GetByIdAsync(partItem.PartId);
            if (part != null)
            {
                part.UpdateStock(partItem.Quantity, StockMovementType.In);
                await _unitOfWork.PartRepository.UpdateAsync(part);
                partsReturned++;
                
                _logger.LogInformation("Returned {Quantity} units of {PartCode} to stock (cancelled order {OrderNumber})", 
                    partItem.Quantity, part.Code, serviceOrder.OrderNumber);
            }
        }

        // Update status to cancelled
        serviceOrder.UpdateStatus(ServiceOrderStatus.Cancelled);
        serviceOrder.InternalNotes = string.IsNullOrWhiteSpace(serviceOrder.InternalNotes)
            ? $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] CANCELADO: {request.Reason}"
            : $"{serviceOrder.InternalNotes}\n[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] CANCELADO: {request.Reason}";

        await _unitOfWork.ServiceOrderRepository.UpdateAsync(serviceOrder);
        await _unitOfWork.CommitAsync(cancellationToken);

        var message = $"Ordem de serviço cancelada. {partsReturned} peças devolvidas ao stock.";

        _logger.LogWarning("Service order cancelled: {OrderNumber}. Reason: {Reason}", 
            serviceOrder.OrderNumber, request.Reason);

        var response = new CancelServiceOrderResponse(
            serviceOrder.Id,
            serviceOrder.OrderNumber,
            partsReturned,
            message
        );

        return response;
    }
}

