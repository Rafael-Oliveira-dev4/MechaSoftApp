using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MechaSoft.Domain.Model;

public class Vehicle : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string LicensePlate { get; set; } // Matrícula portuguesa
    public required string Color { get; set; }
    public int? Mileage { get; set; } // Quilometragem
    public string? ChassisNumber { get; set; }
    public string? EngineNumber { get; set; }
    public FuelType FuelType { get; set; }
    public int? EngineDisplacement { get; set; } // Cilindrada
    public int? EnginePower { get; set; } // Potência em CV

    // Documentação portuguesa
    public DateTime? InspectionExpiryDate { get; set; } // Validade da Inspeção

    // Navigation Properties
    public required Customer Customer { get; set; }
    public List<ServiceOrder> ServiceOrders { get; set; }
    public List<Inspection> Inspections { get; set; }

    public Vehicle()
    {
        Id = Guid.NewGuid();
        ServiceOrders = [];
        Inspections = [];
    }

    public Vehicle(Guid customerId, string brand, string model, int year,
                   string licensePlate, string color, FuelType fuelType,
                   int? mileage = null, string? chassisNumber = null)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        Year = year;
        LicensePlate = ValidateLicensePlate(licensePlate);
        Color = color ?? throw new ArgumentNullException(nameof(color));
        FuelType = fuelType;
        Mileage = mileage;
        ChassisNumber = chassisNumber;
        ServiceOrders = [];
        Inspections = [];
    }

    private static string ValidateLicensePlate(string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("License plate cannot be empty");

        // Formato português: XX-XX-XX ou XX-00-XX
        var cleanPlate = licensePlate.Replace("-", "").Replace(" ", "").ToUpper();
        if (cleanPlate.Length != 6)
            throw new ArgumentException("Invalid Portuguese license plate format");

        return licensePlate.ToUpper();
    }

    public bool IsInspectionExpired()
    {
        return InspectionExpiryDate.HasValue && InspectionExpiryDate.Value < DateTime.Now;
    }

    public bool IsInspectionDueSoon(int daysThreshold = 30)
    {
        return InspectionExpiryDate.HasValue &&
               InspectionExpiryDate.Value <= DateTime.Now.AddDays(daysThreshold);
    }
}

public enum FuelType
{
    Gasoline,   // Gasolina
    Diesel,     // Gasóleo
    Electric,   // Elétrico
    Hybrid,     // Híbrido
    LPG,        // GPL
    CNG         // GNC
}

public enum ServiceItemStatus
{
    Pending,    // Pendente
    InProgress, // Em Progresso
    Paused,     // Pausado
    Completed,  // Concluído
    Cancelled   // Cancelado
}