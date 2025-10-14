using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Accounts.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<Result<UpdateUserProfileResponse, Success, Error>>
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
}

public class UpdateUserProfileResponse
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public DateTime UpdatedAt { get; set; }
}

