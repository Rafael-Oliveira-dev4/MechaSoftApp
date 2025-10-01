using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

    public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetEmployeeByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<EmployeeResponse, Success, Error>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.Id);
        if (employee == null)
        {
            _logger.LogWarning("Employee not found: {EmployeeId}", request.Id);
            return Error.EmployeeNotFound;
        }

        var response = new EmployeeResponse(
            employee.Id,
            employee.Name.FullName,
            employee.Name.FirstName,
            employee.Name.LastName,
            employee.Email,
            employee.Phone,
            employee.Role,
            employee.IsActive,
            employee.CanPerformInspections,
            employee.InspectionLicenseNumber,
            employee.Specialties,
            employee.HourlyRate?.Amount,
            employee.CreatedAt,
            employee.UpdatedAt
        );

        return response;
    }
}

