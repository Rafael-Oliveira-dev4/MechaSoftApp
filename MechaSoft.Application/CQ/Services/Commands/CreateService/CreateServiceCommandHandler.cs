using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Services.Commands.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Result<CreateServiceResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateServiceCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreateServiceResponse, Success, Error>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var pricePerHour = new Money(request.PricePerHour, "EUR");
        Money? fixedPrice = request.FixedPrice.HasValue 
            ? new Money(request.FixedPrice.Value, "EUR") 
            : null;

        var service = new Service(
            request.Name,
            request.Description,
            request.Category,
            request.EstimatedHours,
            pricePerHour,
            fixedPrice
        );

        service.RequiresInspection = request.RequiresInspection;

        var savedService = await _unitOfWork.ServiceRepository.SaveAsync(service);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Service created successfully: {ServiceId}, {ServiceName}", savedService.Id, savedService.Name);

        var response = new CreateServiceResponse(
            savedService.Id,
            savedService.Name,
            savedService.Description,
            savedService.Category,
            savedService.EstimatedHours,
            savedService.PricePerHour.Amount,
            savedService.FixedPrice?.Amount,
            savedService.CreatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

