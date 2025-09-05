using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class User : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Salt { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LockedUntil { get; set; }

    // Navigation Properties
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public User()
    {
        Id = Guid.NewGuid();
        IsActive = true;
        EmailConfirmed = false;
        FailedLoginAttempts = 0;
    }

    public User(string username, string email, string passwordHash, string salt, UserRole role)
    {
        Id = Guid.NewGuid();
        Username = ValidateUsername(username);
        Email = ValidateEmail(email);
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Salt = salt ?? throw new ArgumentNullException(nameof(salt));
        Role = role;
        IsActive = true;
        EmailConfirmed = false;
        FailedLoginAttempts = 0;
    }

    private static string ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (username.Length < 3)
            throw new ArgumentException("Username must have at least 3 characters", nameof(username));

        if (username.Length > 50)
            throw new ArgumentException("Username cannot exceed 50 characters", nameof(username));

        return username.Trim().ToLower();
    }

    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format", nameof(email));

        return email.ToLower().Trim();
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        FailedLoginAttempts = 0;
        LockedUntil = null;
    }

    public void IncrementFailedLoginAttempts()
    {
        FailedLoginAttempts++;
        
        // Lock account after 5 failed attempts for 30 minutes
        if (FailedLoginAttempts >= 5)
        {
            LockedUntil = DateTime.UtcNow.AddMinutes(30);
        }
    }

    public bool IsAccountLocked()
    {
        return LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;
    }

    public void SetRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }

    public void ChangePassword(string newPasswordHash, string newSalt)
    {
        PasswordHash = newPasswordHash;
        Salt = newSalt;
        FailedLoginAttempts = 0;
        LockedUntil = null;
    }

    public void LinkToCustomer(Guid customerId)
    {
        CustomerId = customerId;
        EmployeeId = null;
    }

    public void LinkToEmployee(Guid employeeId)
    {
        EmployeeId = employeeId;
        CustomerId = null;
    }
}

public enum UserRole
{
    Customer,       // Cliente da oficina
    Employee,       // Funcionário da oficina
    Admin,          // Administrador do sistema
    Owner           // Dono da oficina
}
