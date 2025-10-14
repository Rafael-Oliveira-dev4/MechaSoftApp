using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Security.Interfaces;
using MediatR;
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

    public async Task<Result<ChangePasswordResponse, Success, Error>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        // Get user
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("Attempt to change password for non-existent user: {UserId}", request.UserId);
            return new Error("USER_NOT_FOUND", "Utilizador não encontrado");
        }

        // Verify current password
        if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
        {
            _logger.LogWarning("Invalid current password for user: {UserId}", request.UserId);
            return new Error("INVALID_PASSWORD", "Senha atual incorreta");
        }

        // Hash new password
        var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

        // Update password (BCrypt doesn't use separate salt)
        user.ChangePassword(newPasswordHash, null);

        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Password changed successfully for user: {UserId}, {Username}", user.Id, user.Username);

        return new ChangePasswordResponse("Senha alterada com sucesso!");
    }
}
