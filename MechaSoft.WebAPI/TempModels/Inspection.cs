using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("Inspection", Schema = "MechaSoftCS")]
[Index("ExpiryDate", Name = "IX_Inspection_ExpiryDate")]
[Index("InspectionCenter", Name = "IX_Inspection_InspectionCenter")]
[Index("InspectionDate", Name = "IX_Inspection_InspectionDate")]
[Index("Result", Name = "IX_Inspection_Result")]
[Index("Result", "ExpiryDate", Name = "IX_Inspection_Result_Expiry")]
[Index("ServiceOrderId", Name = "IX_Inspection_ServiceOrderId")]
[Index("Type", Name = "IX_Inspection_Type")]
[Index("VehicleId", Name = "IX_Inspection_VehicleId")]
[Index("VehicleId", "InspectionDate", Name = "IX_Inspection_Vehicle_Date")]
[Index("VehicleId", "Type", Name = "IX_Inspection_Vehicle_Type")]
[Index("VehicleId", "Type", "InspectionDate", Name = "IX_Inspection_Vehicle_Type_Date")]
public partial class Inspection
{
    [Key]
    public Guid Id { get; set; }

    public Guid VehicleId { get; set; }

    public Guid ServiceOrderId { get; set; }

    [StringLength(15)]
    public string Type { get; set; } = null!;

    public DateTime InspectionDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    [StringLength(15)]
    public string Result { get; set; } = null!;

    [StringLength(1000)]
    public string? Observations { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostAmount { get; set; }

    [StringLength(3)]
    public string CostCurrency { get; set; } = null!;

    [StringLength(50)]
    public string? CertificateNumber { get; set; }

    [StringLength(200)]
    public string InspectionCenter { get; set; } = null!;

    public int VehicleMileage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("ServiceOrderId")]
    [InverseProperty("Inspections")]
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;

    [ForeignKey("VehicleId")]
    [InverseProperty("Inspections")]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
