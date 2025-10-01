using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;

namespace MechaSoft.Application.CQ.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    EmployeeRole? Role = null,
    bool? IsActive = null,
    string? SearchTerm = null
) : IRequest<Result<GetEmployeesResponse, Success, Error>>;

public record GetEmployeesResponse(
    IEnumerable<EmployeeDto> Employees,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public record EmployeeDto(
    Guid Id,
    string FullName,
    string Email,
    string Phone,
    EmployeeRole Role,
    bool IsActive,
    bool CanPerformInspections,
    string? InspectionLicenseNumber,
    List<ServiceCategory> Specialties,
    decimal? HourlyRate
);

