using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[PrimaryKey("ServiceOrderId", "PartId")]
[Table("PartItem", Schema = "MechaSoftCS")]
[Index("PartId", Name = "IX_PartItem_PartId")]
[Index("ServiceOrderId", Name = "IX_PartItem_ServiceOrderId")]
public partial class PartItem
{
    [Key]
    public Guid ServiceOrderId { get; set; }

    [Key]
    public Guid PartId { get; set; }

    public int Quantity { get; set; }

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

    [ForeignKey("PartId")]
    [InverseProperty("PartItems")]
    public virtual Part Part { get; set; } = null!;

    [ForeignKey("ServiceOrderId")]
    [InverseProperty("PartItems")]
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
}
