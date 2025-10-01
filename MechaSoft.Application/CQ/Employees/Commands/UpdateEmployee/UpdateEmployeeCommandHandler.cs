using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result<UpdateEmployeeResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

    public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdateEmployeeResponse, Success, Error>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.Id);
        if (employee == null)
        {
            _logger.LogWarning("Attempt to update non-existent employee: {EmployeeId}", request.Id);
            return Error.EmployeeNotFound;
        }

        // Update basic properties
        employee.Name = new Name(request.FirstName, request.LastName);
        employee.Email = request.Email;
        employee.Phone = request.Phone;
        employee.Role = request.Role;
        employee.IsActive = request.IsActive;
        employee.CanPerformInspections = request.CanPerformInspections;
        employee.InspectionLicenseNumber = request.InspectionLicenseNumber;

        // Update hourly rate
        if (request.HourlyRate.HasValue)
            employee.HourlyRate = new Money(request.HourlyRate.Value, "EUR");

        // Update specialties
        employee.Specialties.Clear();
        if (request.Specialties != null && request.Specialties.Any())
        {
            foreach (var specialty in request.Specialties)
            {
                employee.AddSpecialty(specialty);
            }
        }

        await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Employee updated successfully: {EmployeeId}, {EmployeeName}", employee.Id, employee.Name.FullName);

        var response = new UpdateEmployeeResponse(
            employee.Id,
            employee.Name.FullName,
            employee.Email,
            employee.IsActive,
            employee.UpdatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

