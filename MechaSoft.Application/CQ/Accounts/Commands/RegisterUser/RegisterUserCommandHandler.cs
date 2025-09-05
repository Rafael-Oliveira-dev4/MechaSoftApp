using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<RegisterUserResponse, Success, Error>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if username already exists
            if (await _unitOfWork.UserRepository.UsernameExistsAsync(request.Username))
            {
                return Error.UsernameAlreadyExists;
            }

            // Check if email already exists
            if (await _unitOfWork.UserRepository.EmailExistsAsync(request.Email))
            {
                return Error.EmailAlreadyExists;
            }

            // Generate salt and hash password
            var salt = GenerateSalt();
            var passwordHash = HashPassword(request.Password, salt);

            // Create user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                Role = request.Role
            };

            // Link to customer or employee if provided
            if (request.CustomerId.HasValue)
            {
                user.LinkToCustomer(request.CustomerId.Value);
            }
            else if (request.EmployeeId.HasValue)
            {
                user.LinkToEmployee(request.EmployeeId.Value);
            }

            // Save user
            var savedUser = await _unitOfWork.UserRepository.SaveAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("User registered successfully: {Username}", request.Username);

            var response = new RegisterUserResponse(
                savedUser.Id,
                savedUser.Username,
                savedUser.Email,
                savedUser.Role,
                savedUser.EmailConfirmed
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Username}", request.Username);
            return Error.OperationFailed;
        }
    }

    private static string GenerateSalt()
    {
        return Guid.NewGuid().ToString("N");
    }

    private static string HashPassword(string password, string salt)
    {
        // Simple hash implementation - in production, use BCrypt or similar
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }
}
