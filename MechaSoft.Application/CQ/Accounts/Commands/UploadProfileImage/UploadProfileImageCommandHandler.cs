using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.Common.Services;
using MechaSoft.Domain.Core.Uow;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.UploadProfileImage;

public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, Result<UploadProfileImageResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileUploadService _fileUploadService;
    private readonly ILogger<UploadProfileImageCommandHandler> _logger;

    public UploadProfileImageCommandHandler(
        IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService,
        ILogger<UploadProfileImageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileUploadService = fileUploadService;
        _logger = logger;
    }

    public async Task<Result<UploadProfileImageResponse, Success, Error>> Handle(
        UploadProfileImageCommand request, 
        CancellationToken cancellationToken)
    {
        // Validate image
        if (!_fileUploadService.IsValidImage(request.FileName, request.FileSize))
        {
            return new Error("InvalidImage", "Invalid image file. Allowed formats: jpg, jpeg, png, webp, gif. Max size: 5MB");
        }

        // Get user
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            return Error.UserNotFound;
        }

        // Delete old profile image if exists
        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
        {
            await _fileUploadService.DeleteProfileImageAsync(user.ProfileImageUrl, cancellationToken);
        }

        // Upload new image
        var imageUrl = await _fileUploadService.UploadProfileImageAsync(
            request.FileStream, 
            request.FileName, 
            cancellationToken);

        // Update user profile
        user.UpdateProfileImage(imageUrl);
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Profile image uploaded for user: {UserId}", request.UserId);

        return new UploadProfileImageResponse(user.Id, imageUrl);
    }
}

