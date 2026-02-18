using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    EmployeeRole Role,
    decimal? HourlyRate,
    List<ServiceCategory>? Specialties,
    bool CanPerformInspections,
    string? InspectionLicenseNumber,
    bool IsActive
) : IRequest<Result<UpdateEmployeeResponse, Success, Error>>;

public record UpdateEmployeeResponse(
    Guid Id,
    string FullName,
    string Email,
    bool IsActive,
    DateTime UpdatedAt
);

