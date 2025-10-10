using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler 
    : IRequestHandler<UpdateUserProfileCommand, Result<UpdateUserProfileResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateUserProfileCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateUserProfileResponse, Success, Error>> Handle(
        UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        // Get user
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", request.UserId);
            return Error.UserNotFound;
        }

        // Check if username changed and if new username already exists (exclude current user)
        if (user.Username != request.Username)
        {
            var existingUserByUsername = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);
            if (existingUserByUsername != null && existingUserByUsername.Id != user.Id)
            {
                _logger.LogWarning("Username already exists: {Username}", request.Username);
                return Error.UsernameAlreadyExists;
            }
        }

        // Check if email changed and if new email already exists (exclude current user)
        if (user.Email != request.Email)
        {
            var existingUserByEmail = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != user.Id)
            {
                _logger.LogWarning("Email already exists: {Email}", request.Email);
                return Error.EmailAlreadyExists;
            }
        }

        // Update user
        user.Username = request.Username;
        user.Email = request.Email;

        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("User profile updated successfully: {UserId}, Username: {Username}", 
            user.Id, user.Username);

        var response = new UpdateUserProfileResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            UpdatedAt = DateTime.UtcNow
        };

        return response;
    }
}

