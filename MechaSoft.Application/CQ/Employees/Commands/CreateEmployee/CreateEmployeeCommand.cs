using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    EmployeeRole Role,
    decimal? HourlyRate,
    List<ServiceCategory>? Specialties,
    bool CanPerformInspections,
    string? InspectionLicenseNumber
) : IRequest<Result<CreateEmployeeResponse, Success, Error>>;

public record CreateEmployeeResponse(
    Guid Id,
    string FullName,
    string Email,
    string Phone,
    EmployeeRole Role,
    DateTime CreatedAt
);

