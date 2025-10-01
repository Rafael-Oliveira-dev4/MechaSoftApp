using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MechaSoft.Security.Interfaces;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<ResetPasswordResponse, Success, Error>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
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

        // Hash new password using BCrypt
        var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

        // Update password (empty salt for BCrypt) and clear reset token
        user.ChangePassword(newPasswordHash, string.Empty);
        user.RevokeRefreshToken();
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Password reset successfully for user: {Email}", request.Email);

        var response = new ResetPasswordResponse(true, "Password reset successfully");
        return response;
    }

}
