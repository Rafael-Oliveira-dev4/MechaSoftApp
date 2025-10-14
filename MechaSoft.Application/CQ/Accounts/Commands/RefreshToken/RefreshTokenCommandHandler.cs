using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MechaSoft.Security.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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
        // Extract userId directly from JWT without validation (we validate refresh token against DB next)
        Guid userId;
        try
        {
            var jwt = new JwtSecurityToken(request.AccessToken);
            var userIdValue = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier
                || c.Type == JwtRegisteredClaimNames.NameId
                || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out userId))
            {
                _logger.LogWarning("Invalid user ID in access token");
                return Error.InvalidToken;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse access token for refresh");
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
