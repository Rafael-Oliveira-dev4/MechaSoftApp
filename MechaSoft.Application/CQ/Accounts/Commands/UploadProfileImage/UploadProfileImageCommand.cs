using MediatR;
using MechaSoft.Application.Common.Responses;

namespace MechaSoft.Application.CQ.Accounts.Commands.UploadProfileImage;

public record UploadProfileImageCommand(
    Guid UserId,
    Stream FileStream,
    string FileName,
    long FileSize
) : IRequest<Result<UploadProfileImageResponse, Success, Error>>;

public record UploadProfileImageResponse(
    Guid UserId,
    string ProfileImageUrl
);

