using System.ComponentModel.DataAnnotations;

namespace MechaSoft.Security.Configuration;

/// <summary>
/// JWT configuration settings
/// </summary>
public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    [Required(ErrorMessage = "JWT Key is required")]
    [MinLength(64, ErrorMessage = "JWT Key must be at least 64 characters for security")]
    public string Key { get; set; } = string.Empty;

    [Required(ErrorMessage = "JWT Issuer is required")]
    public string Issuer { get; set; } = string.Empty;

    [Required(ErrorMessage = "JWT Audience is required")]
    public string Audience { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "Expiration must be between 1 and 1440 minutes (24 hours)")]
    public int ExpirationInMinutes { get; set; } = 60;
}

