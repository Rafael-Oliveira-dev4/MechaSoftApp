using MechaSoft.Application.Common.Responses;
using MediatR;

namespace MechaSoft.Application.CQ.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<Result<EmployeeResponse, Success, Error>>;

public record EmployeeResponse(
    Guid Id,
    string FullName,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    Domain.Model.EmployeeRole Role,
    bool IsActive,
    bool CanPerformInspections,
    string? InspectionLicenseNumber,
    List<Domain.Model.ServiceCategory> Specialties,
    decimal? HourlyRate,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);

