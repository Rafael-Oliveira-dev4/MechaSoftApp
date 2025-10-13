using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("ServiceItem", Schema = "MechaSoftCS")]
[Index("ServiceId", Name = "IX_ServiceItem_ServiceId")]
[Index("ServiceOrderId", Name = "IX_ServiceItem_ServiceOrderId")]
[Index("ServiceOrderId", "Status", Name = "IX_ServiceItem_ServiceOrder_Status")]
[Index("Status", Name = "IX_ServiceItem_Status")]
public partial class ServiceItem
{
    [Key]
    public Guid Id { get; set; }

    public Guid ServiceOrderId { get; set; }

    public Guid ServiceId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal EstimatedHours { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? ActualHours { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPriceAmount { get; set; }

    [StringLength(3)]
    public string UnitPriceCurrency { get; set; } = null!;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? DiscountPercentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalPriceAmount { get; set; }

    [StringLength(3)]
    public string TotalPriceCurrency { get; set; } = null!;

    [StringLength(15)]
    public string Status { get; set; } = null!;

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public Guid? MechanicId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("MechanicId")]
    [InverseProperty("ServiceItems")]
    public virtual Employee? Mechanic { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("ServiceItems")]
    public virtual Service Service { get; set; } = null!;

    [ForeignKey("ServiceOrderId")]
    [InverseProperty("ServiceItems")]
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
}
