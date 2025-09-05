using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ResetPasswordResponse, Success, Error>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user by email
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Reset password attempt with non-existent email: {Email}", request.Email);
                return Error.InvalidResetToken;
            }

            // Validate reset token (simplified - in production, use proper token validation)
            if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshToken != request.ResetToken)
            {
                _logger.LogWarning("Invalid reset token for user: {Email}", request.Email);
                return Error.InvalidResetToken;
            }

            // Check if reset token is expired
            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                _logger.LogWarning("Expired reset token for user: {Email}", request.Email);
                return Error.InvalidResetToken;
            }

            // Generate new salt and hash new password
            var newSalt = GenerateSalt();
            var newPasswordHash = HashPassword(request.NewPassword, newSalt);

            // Update password and clear reset token
            user.ChangePassword(newPasswordHash, newSalt);
            user.RevokeRefreshToken();
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Password reset successfully for user: {Email}", request.Email);

            var response = new ResetPasswordResponse(true, "Password reset successfully");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for email: {Email}", request.Email);
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
