using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;
using MechaSoft.Security.Interfaces;

namespace MechaSoft.Application.CQ.Accounts.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<LoginUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LoginUserResponse, Success, Error>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user by username
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                _logger.LogWarning("Login attempt with non-existent username: {Username}", request.Username);
                return Error.InvalidCredentials;
            }

            // Check if account is locked
            if (user.IsAccountLocked())
            {
                _logger.LogWarning("Login attempt on locked account: {Username}", request.Username);
                return Error.UserLockedOut;
            }

            // Check if account is active
            if (!user.IsActive)
            {
                _logger.LogWarning("Login attempt on inactive account: {Username}", request.Username);
                return Error.AccountInactive;
            }

            // Verify password
            var hashedPassword = HashPassword(request.Password, user.Salt);
            if (hashedPassword != user.PasswordHash)
            {
                user.IncrementFailedLoginAttempts();
                await _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync(cancellationToken);
                
                _logger.LogWarning("Invalid password for user: {Username}", request.Username);
                return Error.InvalidCredentials;
            }

            // Update last login and reset failed attempts
            user.UpdateLastLogin();

            // Generate tokens
            var accessToken = _unitOfWork.TokenService.GenerateToken(user);
            var refreshToken = _unitOfWork.TokenService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddHours(1);

            // Set refresh token
            user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("User logged in successfully: {Username}", request.Username);

            var response = new LoginUserResponse(
                user.Id,
                user.Username,
                user.Email,
                user.Role,
                accessToken,
                refreshToken,
                expiresAt
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Username}", request.Username);
            return Error.OperationFailed;
        }
    }


    private static string HashPassword(string password, string salt)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }
}
