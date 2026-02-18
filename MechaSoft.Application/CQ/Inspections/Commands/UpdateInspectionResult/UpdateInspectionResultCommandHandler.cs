using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Inspections.Commands.UpdateInspectionResult;

public class UpdateInspectionResultCommandHandler : IRequestHandler<UpdateInspectionResultCommand, Result<UpdateInspectionResultResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateInspectionResultCommandHandler> _logger;

    public UpdateInspectionResultCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateInspectionResultCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateInspectionResultResponse, Success, Error>> Handle(UpdateInspectionResultCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _unitOfWork.InspectionRepository.GetByIdAsync(request.InspectionId);
        if (inspection == null)
        {
            _logger.LogWarning("Attempt to update non-existent inspection: {InspectionId}", request.InspectionId);
            return Error.InspectionNotFound;
        }

        var oldResult = inspection.Result;

        // Update result
        inspection.Result = request.Result;
        inspection.CertificateNumber = request.CertificateNumber;
        
        if (!string.IsNullOrWhiteSpace(request.Observations))
        {
            inspection.Observations = string.IsNullOrWhiteSpace(inspection.Observations)
                ? request.Observations
                : $"{inspection.Observations}\n{request.Observations}";
        }

        await _unitOfWork.InspectionRepository.UpdateAsync(inspection);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Inspection result updated: {InspectionId}, {OldResult} -> {NewResult}", 
            inspection.Id, oldResult, request.Result);

        var response = new UpdateInspectionResultResponse(
            inspection.Id,
            inspection.Result,
            inspection.CertificateNumber,
            inspection.UpdatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}
