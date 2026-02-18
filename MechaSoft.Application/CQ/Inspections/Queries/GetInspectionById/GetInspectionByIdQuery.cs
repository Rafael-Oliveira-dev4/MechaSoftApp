using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Inspections.Queries.GetInspectionById;

public record GetInspectionByIdQuery(Guid Id) : IRequest<Result<InspectionResponse, Success, Error>>;

public record InspectionResponse(
    Guid Id,
    Guid VehicleId,
    string VehicleInfo,
    Guid ServiceOrderId,
    string ServiceOrderNumber,
    InspectionType Type,
    DateTime InspectionDate,
    DateTime ExpiryDate,
    InspectionResult Result,
    string? Observations,
    decimal Cost,
    string? CertificateNumber,
    string InspectionCenter,
    int VehicleMileage,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);

