using MechaSoft.Domain.Model;

namespace MechaSoft.Application.CQ.Employees.Common;

public record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    EmployeeRole Role,
    decimal? HourlyRate,
    List<ServiceCategory>? Specialties,
    bool CanPerformInspections,
    string? InspectionLicenseNumber
);


