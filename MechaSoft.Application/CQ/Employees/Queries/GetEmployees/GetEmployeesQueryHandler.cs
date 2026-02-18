using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Result<GetEmployeesResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetEmployeesQueryHandler> _logger;

    public GetEmployeesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetEmployeesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetEmployeesResponse, Success, Error>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var (employees, totalCount) = await _unitOfWork.EmployeeRepository.GetPagedEmployeesAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.Role,
            request.IsActive,
            null
        );

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var employeeDtos = employees.Select(e => new EmployeeDto(
            e.Id,
            e.Name.FullName,
            e.Email,
            e.Phone,
            e.Role,
            e.IsActive,
            e.CanPerformInspections,
            e.InspectionLicenseNumber,
            e.Specialties,
            e.HourlyRate?.Amount
        ));

        var response = new GetEmployeesResponse(
            employeeDtos,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages
        );

        return response;
    }
}

