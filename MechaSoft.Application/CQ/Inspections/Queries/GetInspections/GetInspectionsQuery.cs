using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Inspections.Queries.GetInspections;

public record GetInspectionsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? VehicleId = null,
    InspectionResult? Result = null
) : IRequest<Result<GetInspectionsResponse, Success, Error>>;

public record GetInspectionsResponse(
    IEnumerable<InspectionDto> Inspections,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public record InspectionDto(
    Guid Id,
    Guid VehicleId,
    string VehicleInfo,
    InspectionType Type,
    DateTime InspectionDate,
    DateTime ExpiryDate,
    InspectionResult Result,
    decimal Cost,
    string InspectionCenter,
    int VehicleMileage,
    string? CertificateNumber
);

