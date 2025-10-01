using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MechaSoft.Security.Interfaces;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<ChangePasswordResponse, Success, Error>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Get user
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Error.UserNotFound;

        // Verify current password (supports both BCrypt and legacy SHA256)
        bool isCurrentPasswordValid = VerifyPassword(request.CurrentPassword, user.PasswordHash, user.Salt);
        if (!isCurrentPasswordValid)
        {
            _logger.LogWarning("Invalid current password for user: {UserId}", request.UserId);
            return Error.InvalidPassword;
        }

        // Hash new password using BCrypt
        var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

        // Update password (empty salt for BCrypt)
        user.ChangePassword(newPasswordHash, string.Empty);
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Password changed successfully for user: {UserId}", request.UserId);

        var response = new ChangePasswordResponse(true, "Password changed successfully");
        return response;
    }

    /// <summary>
    /// Verifies password supporting both BCrypt (new) and SHA256 (legacy) hashes
    /// </summary>
    private bool VerifyPassword(string password, string storedHash, string? salt)
    {
        // New BCrypt hash (no salt stored separately)
        if (string.IsNullOrEmpty(salt))
        {
            return _passwordHasher.VerifyPassword(password, storedHash);
        }

        // Legacy SHA256 hash with salt
        try
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var saltedPassword = password + salt;
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
            var computedHash = Convert.ToBase64String(hashedBytes);
            return computedHash == storedHash;
        }
        catch
        {
            return false;
        }
    }
}
