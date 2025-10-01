using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, Result<UpdateServiceResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateServiceCommandHandler> _logger;

    public UpdateServiceCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateServiceCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateServiceResponse, Success, Error>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id);
        if (service == null)
        {
            _logger.LogWarning("Attempt to update non-existent service: {ServiceId}", request.Id);
            return Error.ServiceNotFound;
        }

        // Update properties
        service.Name = request.Name;
        service.Description = request.Description;
        service.Category = request.Category;
        service.EstimatedHours = request.EstimatedHours;
        service.PricePerHour = new Money(request.PricePerHour, "EUR");
        service.FixedPrice = request.FixedPrice.HasValue ? new Money(request.FixedPrice.Value, "EUR") : null;
        service.RequiresInspection = request.RequiresInspection;
        service.IsActive = request.IsActive;

        await _unitOfWork.ServiceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Service updated successfully: {ServiceId}, {ServiceName}", service.Id, service.Name);

        var response = new UpdateServiceResponse(
            service.Id,
            service.Name,
            service.IsActive,
            service.UpdatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

