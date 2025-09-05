using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ChangePasswordResponse, Success, Error>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Error.UserNotFound;
            }

            // Verify current password
            var currentPasswordHash = HashPassword(request.CurrentPassword, user.Salt);
            if (currentPasswordHash != user.PasswordHash)
            {
                _logger.LogWarning("Invalid current password for user: {UserId}", request.UserId);
                return Error.InvalidPassword;
            }

            // Generate new salt and hash new password
            var newSalt = GenerateSalt();
            var newPasswordHash = HashPassword(request.NewPassword, newSalt);

            // Update password
            user.ChangePassword(newPasswordHash, newSalt);
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Password changed successfully for user: {UserId}", request.UserId);

            var response = new ChangePasswordResponse(true, "Password changed successfully");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", request.UserId);
            return Error.OperationFailed;
        }
    }

    private static string GenerateSalt()
    {
        return Guid.NewGuid().ToString("N");
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }
}
