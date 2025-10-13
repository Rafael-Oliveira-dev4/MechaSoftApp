using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("Service", Schema = "MechaSoftCS")]
[Index("Category", Name = "IX_Service_Category")]
[Index("Category", "IsActive", Name = "IX_Service_Category_IsActive")]
[Index("IsActive", Name = "IX_Service_IsActive")]
[Index("Name", Name = "IX_Service_Name", IsUnique = true)]
public partial class Service
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [StringLength(30)]
    public string Category { get; set; } = null!;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal EstimatedHours { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PricePerHourAmount { get; set; }

    [StringLength(3)]
    public string PricePerHourCurrency { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? FixedPriceAmount { get; set; }

    [StringLength(3)]
    public string? FixedPriceCurrency { get; set; }

    public bool IsActive { get; set; }

    public bool RequiresInspection { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();
}
