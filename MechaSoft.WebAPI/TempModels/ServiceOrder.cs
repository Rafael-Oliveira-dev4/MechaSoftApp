using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("ServiceOrder", Schema = "MechaSoftCS")]
[Index("CreatedAt", Name = "IX_ServiceOrder_CreatedAt")]
[Index("CustomerId", Name = "IX_ServiceOrder_CustomerId")]
[Index("OrderNumber", Name = "IX_ServiceOrder_OrderNumber", IsUnique = true)]
[Index("Priority", Name = "IX_ServiceOrder_Priority")]
[Index("RequiresInspection", Name = "IX_ServiceOrder_RequiresInspection")]
[Index("Status", Name = "IX_ServiceOrder_Status")]
[Index("Status", "Priority", Name = "IX_ServiceOrder_Status_Priority")]
[Index("VehicleId", Name = "IX_ServiceOrder_VehicleId")]
public partial class ServiceOrder
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(20)]
    public string OrderNumber { get; set; } = null!;

    public Guid CustomerId { get; set; }

    public Guid VehicleId { get; set; }

    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(10)]
    public string Priority { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal EstimatedCostAmount { get; set; }

    [StringLength(3)]
    public string EstimatedCostCurrency { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? FinalCostAmount { get; set; }

    [StringLength(3)]
    public string? FinalCostCurrency { get; set; }

    public DateTime? EstimatedDelivery { get; set; }

    public DateTime? ActualDelivery { get; set; }

    public Guid? MechanicId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? ActualHours { get; set; }

    public bool RequiresInspection { get; set; }

    [StringLength(2000)]
    public string? InternalNotes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("ServiceOrders")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("ServiceOrder")]
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    [ForeignKey("MechanicId")]
    [InverseProperty("ServiceOrders")]
    public virtual Employee? Mechanic { get; set; }

    [InverseProperty("ServiceOrder")]
    public virtual ICollection<PartItem> PartItems { get; set; } = new List<PartItem>();

    [InverseProperty("ServiceOrder")]
    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

    [ForeignKey("VehicleId")]
    [InverseProperty("ServiceOrders")]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
