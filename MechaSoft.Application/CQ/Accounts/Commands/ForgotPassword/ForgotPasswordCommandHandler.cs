using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<ForgotPasswordResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ForgotPasswordResponse, Success, Error>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user by email
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                // For security reasons, don't reveal if email exists or not
                _logger.LogInformation("Forgot password request for non-existent email: {Email}", request.Email);
                return 
                    new ForgotPasswordResponse(true, "If the email exists, a reset link has been sent");
            }

            // Check if account is active
            if (!user.IsActive)
            {
                _logger.LogWarning("Forgot password request for inactive account: {Email}", request.Email);
                return 
                    new ForgotPasswordResponse(true, "If the email exists, a reset link has been sent");
            }

            // Generate reset token (simplified - in production, use proper token generation)
            var resetToken = GenerateResetToken();
            var expiryTime = DateTime.UtcNow.AddHours(1); // Reset token expires in 1 hour

            // Set reset token
            user.SetRefreshToken(resetToken, expiryTime);
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            // In a real application, you would send an email here
            // For now, we'll just log the token (remove this in production)
            _logger.LogInformation("Reset token generated for user {Email}: {ResetToken}", request.Email, resetToken);

            _logger.LogInformation("Password reset requested for user: {Email}", request.Email);

            return 
                new ForgotPasswordResponse(true, "If the email exists, a reset link has been sent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing forgot password request for email: {Email}", request.Email);
            return Error.OperationFailed;
        }
    }

    private static string GenerateResetToken()
    {
        // In production, use a proper token generation method
        return Guid.NewGuid().ToString("N");
    }
}
