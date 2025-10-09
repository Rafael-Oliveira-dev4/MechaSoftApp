using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<CreateEmployeeResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreateEmployeeResponse, Success, Error>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        var existingEmployee = await _unitOfWork.EmployeeRepository.GetByEmailAsync(request.Email);
        if (existingEmployee != null)
        {
            _logger.LogWarning("Attempt to create employee with existing email: {Email}", request.Email);
            return Error.ExistingEmployee;
        }

        // Create hourly rate Money object if provided
        Money? hourlyRate = request.HourlyRate.HasValue 
            ? new Money(request.HourlyRate.Value, "EUR") 
            : null;

        // Create employee
        var employee = new Employee(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.Role,
            hourlyRate
        );

        // Set specialties if provided
        if (request.Specialties != null && request.Specialties.Any())
        {
            foreach (var specialty in request.Specialties)
            {
                employee.AddSpecialty(specialty);
            }
        }

        // Set inspection details
        employee.CanPerformInspections = request.CanPerformInspections;
        employee.InspectionLicenseNumber = request.InspectionLicenseNumber;

        // Save employee
        var savedEmployee = await _unitOfWork.EmployeeRepository.SaveAsync(employee);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Employee created successfully: {EmployeeId}, {EmployeeName}", 
            savedEmployee.Id, savedEmployee.Name.FullName);

        var response = new CreateEmployeeResponse(
            savedEmployee.Id,
            savedEmployee.Name.FullName,
            savedEmployee.Email,
            savedEmployee.Phone,
            savedEmployee.Role,
            savedEmployee.CreatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

