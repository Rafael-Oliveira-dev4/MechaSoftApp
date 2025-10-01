using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Parts.Commands.UpdateStock;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, Result<UpdateStockResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateStockCommandHandler> _logger;

    public UpdateStockCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateStockCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateStockResponse, Success, Error>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.PartRepository.GetByIdAsync(request.PartId);
        if (part == null)
        {
            _logger.LogWarning("Attempt to update stock for non-existent part: {PartId}", request.PartId);
            return Error.PartNotFound;
        }

        // Validate stock before update
        if (request.MovementType == Domain.Model.StockMovementType.Out && part.StockQuantity < request.Quantity)
        {
            _logger.LogWarning("Insufficient stock for part {PartCode}: Requested: {Requested}, Available: {Available}", 
                part.Code, request.Quantity, part.StockQuantity);
            return Error.InsufficientStock;
        }

        var oldStock = part.StockQuantity;

        // Update stock (won't throw exception now because we validated)
        part.UpdateStock(request.Quantity, request.MovementType);

        await _unitOfWork.PartRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync(cancellationToken);

        var movementDescription = request.MovementType == Domain.Model.StockMovementType.In ? "added to" : "removed from";
        var message = $"{request.Quantity} units {movementDescription} stock. Previous: {oldStock}, Current: {part.StockQuantity}";

        if (!string.IsNullOrWhiteSpace(request.Reason))
            message += $". Reason: {request.Reason}";

        _logger.LogInformation("Stock updated for part {PartId} ({PartCode}): {Message}", 
            part.Id, part.Code, message);

        if (part.IsLowStock())
        {
            _logger.LogWarning("Low stock alert for part {PartId} ({PartCode}): {StockQuantity}/{MinStockLevel}", 
                part.Id, part.Code, part.StockQuantity, part.MinStockLevel);
        }

        var response = new UpdateStockResponse(
            part.Id,
            part.StockQuantity,
            part.IsLowStock(),
            message
        );

        return response;
    }
}

