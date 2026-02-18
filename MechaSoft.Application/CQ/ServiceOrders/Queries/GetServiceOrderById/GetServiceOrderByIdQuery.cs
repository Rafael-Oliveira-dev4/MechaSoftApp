using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.ServiceOrders.Queries.GetServiceOrderById;

public record GetServiceOrderByIdQuery(Guid Id) : IRequest<Result<ServiceOrderResponse, Success, Error>>;

public record ServiceOrderResponse(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    string CustomerName,
    Guid VehicleId,
    string VehicleInfo,
    string Description,
    ServiceOrderStatus Status,
    Priority Priority,
    decimal EstimatedCost,
    decimal? FinalCost,
    DateTime? EstimatedDelivery,
    DateTime? ActualDelivery,
    Guid? MechanicId,
    string? MechanicName,
    decimal? ActualHours,
    bool RequiresInspection,
    string? InternalNotes,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<ServiceItemDto> Services,
    List<PartItemDto> Parts
);

public record ServiceItemDto(
    Guid Id,
    Guid ServiceId,
    string ServiceName,
    int Quantity,
    decimal EstimatedHours,
    decimal UnitPrice,
    decimal? DiscountPercentage,
    decimal TotalPrice,
    string Status,
    Guid? MechanicId,
    string? MechanicName
);

public record PartItemDto(
    Guid PartId,
    string PartName,
    string PartCode,
    int Quantity,
    decimal UnitPrice,
    decimal? DiscountPercentage,
    decimal TotalPrice
);

