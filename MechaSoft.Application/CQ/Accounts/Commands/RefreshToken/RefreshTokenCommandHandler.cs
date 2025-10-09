using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MechaSoft.Security.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MechaSoft.Application.CQ.Accounts.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<RefreshTokenResponse, Success, Error>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Get principal from expired token
        var principal = _unitOfWork.TokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
        {
            _logger.LogWarning("Invalid access token provided for refresh");
            return Error.InvalidToken;
        }

        // Get user ID from claims
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            _logger.LogWarning("Invalid user ID in access token");
            return Error.InvalidToken;
        }

        // Get user
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found for refresh token: {UserId}", userId);
            return Error.InvalidToken;
        }

        // Check if refresh token matches
        if (user.RefreshToken != request.RefreshToken)
        {
            _logger.LogWarning("Invalid refresh token for user: {UserId}", userId);
            return Error.InvalidToken;
        }

        // Check if refresh token is expired
        if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            _logger.LogWarning("Expired refresh token for user: {UserId}", userId);
            return Error.InvalidToken;
        }

        // Check if account is active
        if (!user.IsActive)
        {
            _logger.LogWarning("Inactive account attempting refresh: {UserId}", userId);
            return Error.InvalidToken;
        }

        // Generate new tokens
        var newAccessToken = _unitOfWork.TokenService.GenerateToken(user);
        var newRefreshToken = _unitOfWork.TokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(1);

        // Update refresh token
        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Tokens refreshed successfully for user: {UserId}", userId);

        var response = new RefreshTokenResponse(
            newAccessToken,
            newRefreshToken,
            expiresAt
        );

        return response;
    }
}
