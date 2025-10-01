using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.UpdateServiceOrderStatus;

public class UpdateServiceOrderStatusCommandHandler : IRequestHandler<UpdateServiceOrderStatusCommand, Result<UpdateServiceOrderStatusResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateServiceOrderStatusCommandHandler> _logger;

    public UpdateServiceOrderStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateServiceOrderStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateServiceOrderStatusResponse, Success, Error>> Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (serviceOrder == null)
        {
            _logger.LogWarning("Attempt to update status of non-existent service order: {ServiceOrderId}", request.ServiceOrderId);
            return Error.ServiceOrderNotFound;
        }

        var oldStatus = serviceOrder.Status;
        
        // Update status
        serviceOrder.UpdateStatus(request.NewStatus);

        // Add notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            serviceOrder.InternalNotes = string.IsNullOrWhiteSpace(serviceOrder.InternalNotes)
                ? request.Notes
                : $"{serviceOrder.InternalNotes}\n[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] {request.Notes}";
        }

        await _unitOfWork.ServiceOrderRepository.UpdateAsync(serviceOrder);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Service order status updated: {OrderNumber}, {OldStatus} -> {NewStatus}", 
            serviceOrder.OrderNumber, oldStatus, request.NewStatus);

        var response = new UpdateServiceOrderStatusResponse(
            serviceOrder.Id,
            serviceOrder.OrderNumber,
            serviceOrder.Status,
            serviceOrder.UpdatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}
