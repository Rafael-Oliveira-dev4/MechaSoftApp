using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class Employee : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public Name Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public EmployeeRole Role { get; set; }
    public List<ServiceCategory> Specialties { get; set; }
    public Money? HourlyRate { get; set; }
    public bool IsActive { get; set; }
    public bool CanPerformInspections { get; set; } // Se pode fazer inspeções
    public string? InspectionLicenseNumber { get; set; } // Número da licença para inspeções

    // Navigation Properties
    public List<ServiceOrder> ServiceOrders { get; set; }

    public Employee()
    {
        Id = Guid.NewGuid();
        Name = null!;
        Email = Phone = string.Empty;
        Specialties = [];
        ServiceOrders = [];
        IsActive = true;
    }

    public Employee(string firstName, string lastName, string email, string phone,
                   EmployeeRole role, Money? hourlyRate = null)
    {
        Id = Guid.NewGuid();
        Name = new(firstName, lastName);
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        Role = role;
        HourlyRate = hourlyRate;
        Specialties = [];
        ServiceOrders = [];
        IsActive = true;

        // Se for dono, pode fazer tudo
        if (role == EmployeeRole.Owner)
        {
            CanPerformInspections = true;
            Specialties = Enum.GetValues<ServiceCategory>().ToList();
        }
    }

    public void AddSpecialty(ServiceCategory specialty)
    {
        if (!Specialties.Contains(specialty))
        {
            Specialties.Add(specialty);
        }
    }

    public void RemoveSpecialty(ServiceCategory specialty)
    {
        Specialties.Remove(specialty);
    }
}
public enum EmployeeRole
{
    Owner,          // Dono (faz tudo)
    Mechanic,       // Mecânico
    Manager,        // Gerente
    Receptionist,   // Rececionista
    PartsClerk      // Responsável pelas peças
}

