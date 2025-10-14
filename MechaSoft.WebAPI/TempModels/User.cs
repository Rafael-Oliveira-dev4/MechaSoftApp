using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("Users", Schema = "MechaSoftCS")]
[Index("CustomerId", Name = "IX_Users_CustomerId")]
[Index("Email", Name = "IX_Users_Email", IsUnique = true)]
[Index("EmployeeId", Name = "IX_Users_EmployeeId")]
[Index("Username", Name = "IX_Users_Username", IsUnique = true)]
public partial class User
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(50)]
    public string? Salt { get; set; }

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool EmailConfirmed { get; set; }

    public DateTime? LastLoginAt { get; set; }

    [StringLength(255)]
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? LockedUntil { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? EmployeeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public string? ProfileImageUrl { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Users")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Users")]
    public virtual Employee? Employee { get; set; }
}
