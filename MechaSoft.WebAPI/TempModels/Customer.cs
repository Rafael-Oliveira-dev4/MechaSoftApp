using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.WebAPI.TempModels;

[Table("Customer", Schema = "MechaSoftCS")]
[Index("Email", Name = "IX_Customer_Email", IsUnique = true)]
[Index("Phone", Name = "IX_Customer_Phone")]
public partial class Customer
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    [StringLength(9)]
    public string? Nif { get; set; }

    [StringLength(20)]
    public string? CitizenCard { get; set; }

    [StringLength(200)]
    public string Street { get; set; } = null!;

    [StringLength(20)]
    public string Number { get; set; } = null!;

    [StringLength(100)]
    public string Parish { get; set; } = null!;

    [StringLength(100)]
    public string Municipality { get; set; } = null!;

    [StringLength(100)]
    public string District { get; set; } = null!;

    [StringLength(8)]
    public string PostalCode { get; set; } = null!;

    [StringLength(100)]
    public string? Complement { get; set; }

    [StringLength(20)]
    public string Type { get; set; } = null!;

    [StringLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

    [InverseProperty("Customer")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [InverseProperty("Customer")]
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
