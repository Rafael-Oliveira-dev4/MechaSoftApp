using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Customers.Commands.ToggleCustomerActive;

/// <summary>
/// Soft-delete command - Toggles customer active status instead of deleting
/// </summary>
public record ToggleCustomerActiveCommand(
    Guid CustomerId,
    bool IsActive,
    string? Reason
) : IRequest<Result<ToggleCustomerActiveResponse, Success, Error>>;

public record ToggleCustomerActiveResponse(
    Guid CustomerId,
    string CustomerName,
    bool IsActive,
    string Message
);

