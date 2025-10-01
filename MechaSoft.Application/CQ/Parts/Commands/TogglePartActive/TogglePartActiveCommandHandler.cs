using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Parts.Commands.TogglePartActive;

public class TogglePartActiveCommandHandler : IRequestHandler<TogglePartActiveCommand, Result<TogglePartActiveResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TogglePartActiveCommandHandler> _logger;

    public TogglePartActiveCommandHandler(IUnitOfWork unitOfWork, ILogger<TogglePartActiveCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<TogglePartActiveResponse, Success, Error>> Handle(TogglePartActiveCommand request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.PartRepository.GetByIdAsync(request.PartId);
        if (part == null)
        {
            _logger.LogWarning("Attempt to toggle active status for non-existent part: {PartId}", request.PartId);
            return Error.PartNotFound;
        }

        part.IsActive = request.IsActive;

        await _unitOfWork.PartRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync(cancellationToken);

        var action = request.IsActive ? "reativada" : "desativada";
        var message = $"Peça {action} com sucesso";
        
        if (!string.IsNullOrWhiteSpace(request.Reason))
            message += $". Motivo: {request.Reason}";

        _logger.LogInformation("Part {Action}: {PartCode}. Reason: {Reason}", 
            action, part.Code, request.Reason ?? "N/A");

        var response = new TogglePartActiveResponse(
            part.Id,
            part.Code,
            part.IsActive,
            message
        );

        return response;
    }
}

