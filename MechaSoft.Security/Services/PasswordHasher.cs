using MechaSoft.Security.Interfaces;

namespace MechaSoft.Security.Services;

/// <summary>
/// BCrypt-based password hasher implementation
/// Note: BCrypt automatically handles salt generation and verification
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    // BCrypt work factor (cost) - higher is more secure but slower
    // 12 is a good balance between security and performance in 2024
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        // BCrypt automatically generates and includes salt in the hash
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be null or empty", nameof(hash));

        try
        {
            // BCrypt automatically extracts salt from hash and verifies
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            // Invalid hash format or other BCrypt error
            return false;
        }
    }
}

