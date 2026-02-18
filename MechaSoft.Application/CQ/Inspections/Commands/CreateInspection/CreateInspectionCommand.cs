using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Inspections.Commands.CreateInspection;

public record CreateInspectionCommand(
    Guid VehicleId,
    Guid ServiceOrderId,
    InspectionType Type,
    DateTime InspectionDate,
    DateTime ExpiryDate,
    decimal Cost,
    string InspectionCenter,
    int VehicleMileage,
    string? Observations
) : IRequest<Result<CreateInspectionResponse, Success, Error>>;

public record CreateInspectionResponse(
    Guid Id,
    Guid VehicleId,
    InspectionType Type,
    DateTime InspectionDate,
    InspectionResult Result,
    DateTime CreatedAt
);

