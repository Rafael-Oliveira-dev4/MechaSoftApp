using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Parts.Commands.TogglePartActive;

/// <summary>
/// Soft-delete for parts - preserves history
/// </summary>
public record TogglePartActiveCommand(
    Guid PartId,
    bool IsActive,
    string? Reason
) : IRequest<Result<TogglePartActiveResponse, Success, Error>>;

public record TogglePartActiveResponse(
    Guid PartId,
    string PartCode,
    bool IsActive,
    string Message
);

