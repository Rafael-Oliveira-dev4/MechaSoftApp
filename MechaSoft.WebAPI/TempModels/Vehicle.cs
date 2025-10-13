using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("Vehicle", Schema = "MechaSoftCS")]
[Index("Brand", "Model", Name = "IX_Vehicle_Brand_Model")]
[Index("CustomerId", Name = "IX_Vehicle_CustomerId")]
[Index("FuelType", Name = "IX_Vehicle_FuelType")]
[Index("LicensePlate", Name = "IX_Vehicle_LicensePlate", IsUnique = true)]
[Index("Year", Name = "IX_Vehicle_Year")]
public partial class Vehicle
{
    [Key]
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    [StringLength(100)]
    public string Brand { get; set; } = null!;

    [StringLength(100)]
    public string Model { get; set; } = null!;

    public int Year { get; set; }

    [StringLength(10)]
    public string LicensePlate { get; set; } = null!;

    [StringLength(50)]
    public string Color { get; set; } = null!;

    public int? Mileage { get; set; }

    [StringLength(50)]
    public string? ChassisNumber { get; set; }

    [StringLength(50)]
    public string? EngineNumber { get; set; }

    [StringLength(15)]
    public string FuelType { get; set; } = null!;

    public int? EngineDisplacement { get; set; }

    public int? EnginePower { get; set; }

    public DateTime? InspectionExpiryDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Vehicles")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Vehicle")]
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    [InverseProperty("Vehicle")]
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
}
