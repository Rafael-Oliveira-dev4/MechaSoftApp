using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Inspections.Commands.UpdateInspectionResult;

public record UpdateInspectionResultCommand(
    Guid InspectionId,
    InspectionResult Result,
    string? CertificateNumber,
    string? Observations
) : IRequest<Result<UpdateInspectionResultResponse, Success, Error>>;

public record UpdateInspectionResultResponse(
    Guid Id,
    InspectionResult Result,
    string? CertificateNumber,
    DateTime UpdatedAt
);

