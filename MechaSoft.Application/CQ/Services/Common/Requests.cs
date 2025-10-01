using MechaSoft.Domain.Model;

namespace MechaSoft.Application.CQ.Services.Common;

public record CreateServiceRequest(
    string Name,
    string Description,
    ServiceCategory Category,
    decimal EstimatedHours,
    decimal PricePerHour,
    decimal? FixedPrice,
    bool RequiresInspection
);


