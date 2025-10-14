using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("Part", Schema = "MechaSoftCS")]
[Index("Category", Name = "IX_Part_Category")]
[Index("Category", "IsActive", Name = "IX_Part_Category_IsActive")]
[Index("Code", Name = "IX_Part_Code", IsUnique = true)]
[Index("IsActive", Name = "IX_Part_IsActive")]
[Index("Name", Name = "IX_Part_Name")]
[Index("StockQuantity", "MinStockLevel", Name = "IX_Part_Stock_Levels")]
public partial class Part
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [StringLength(100)]
    public string Category { get; set; } = null!;

    [StringLength(100)]
    public string? Brand { get; set; }

    public int StockQuantity { get; set; }

    public int MinStockLevel { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitCostAmount { get; set; }

    [StringLength(3)]
    public string UnitCostCurrency { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SalePriceAmount { get; set; }

    [StringLength(3)]
    public string SalePriceCurrency { get; set; } = null!;

    [StringLength(200)]
    public string? SupplierName { get; set; }

    [StringLength(255)]
    public string? SupplierContact { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Part")]
    public virtual ICollection<PartItem> PartItems { get; set; } = new List<PartItem>();
}
