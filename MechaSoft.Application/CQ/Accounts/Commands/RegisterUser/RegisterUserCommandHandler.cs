using MediatR;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MechaSoft.Security.Interfaces;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Accounts.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<RegisterUserResponse, Success, Error>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if username already exists
        if (await _unitOfWork.UserRepository.UsernameExistsAsync(request.Username))
            return Error.UsernameAlreadyExists;

        // Check if email already exists
        if (await _unitOfWork.UserRepository.EmailExistsAsync(request.Email))
            return Error.EmailAlreadyExists;

        // Hash password using BCrypt (salt is handled internally)
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // Create user
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            Salt = string.Empty, // BCrypt handles salt internally
            Role = request.Role
        };

        // Link to customer or employee if provided
        if (request.CustomerId.HasValue)
            user.LinkToCustomer(request.CustomerId.Value);
        else if (request.EmployeeId.HasValue)
            user.LinkToEmployee(request.EmployeeId.Value);

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

}
