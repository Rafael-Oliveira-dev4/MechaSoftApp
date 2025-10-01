using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, Result<ServiceResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetServiceByIdQueryHandler> _logger;

    public GetServiceByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetServiceByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ServiceResponse, Success, Error>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id);
        if (service == null)
        {
            _logger.LogWarning("Service not found: {ServiceId}", request.Id);
            return Error.ServiceNotFound;
        }

        var response = new ServiceResponse(
            service.Id,
            service.Name,
            service.Description,
            service.Category,
            service.EstimatedHours,
            service.PricePerHour.Amount,
            service.FixedPrice?.Amount,
            service.IsActive,
            service.RequiresInspection,
            service.CreatedAt,
            service.UpdatedAt
        );

        return response;
    }
}

