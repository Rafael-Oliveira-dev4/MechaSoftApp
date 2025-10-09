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
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<LoginUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<LoginUserResponse, Success, Error>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
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

        // Verify password (supports both BCrypt and legacy SHA256)
        bool isPasswordValid = VerifyPassword(request.Password, user.PasswordHash, user.Salt);
        
        if (!isPasswordValid)
        {
            user.IncrementFailedLoginAttempts();
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            _logger.LogWarning("Invalid password for user: {Username}", request.Username);
            return Error.InvalidCredentials;
        }

        // Upgrade legacy SHA256 hash to BCrypt on successful login
        if (!string.IsNullOrEmpty(user.Salt))
        {
            var newHash = _passwordHasher.HashPassword(request.Password);
            user.ChangePassword(newHash, string.Empty); // Empty salt for BCrypt
            _logger.LogInformation("Upgraded password hash to BCrypt for user: {Username}", request.Username);
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
