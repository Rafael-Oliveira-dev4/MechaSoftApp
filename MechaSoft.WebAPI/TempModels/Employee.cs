using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("Employee", Schema = "MechaSoftCS")]
[Index("Email", Name = "IX_Employee_Email", IsUnique = true)]
[Index("IsActive", Name = "IX_Employee_IsActive")]
[Index("Role", Name = "IX_Employee_Role")]
public partial class Employee
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [StringLength(500)]
    public string Specialties { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? HourlyRateAmount { get; set; }

    [StringLength(3)]
    public string? HourlyRateCurrency { get; set; }

    public bool IsActive { get; set; }

    public bool CanPerformInspections { get; set; }

    [StringLength(50)]
    public string? InspectionLicenseNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Mechanic")]
    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

    [InverseProperty("Mechanic")]
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

    [InverseProperty("Employee")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
