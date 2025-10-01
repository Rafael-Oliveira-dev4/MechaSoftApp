using MechaSoft.Application.CQ.Employees.Commands.CreateEmployee;
using MechaSoft.Application.CQ.Employees.Queries.GetEmployeeById;
using MechaSoft.Application.CQ.Employees.Queries.GetEmployees;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class EmployeeEndpoints
{
    public static void RegisterEmployeeEndpoints(this IEndpointRouteBuilder routes)
    {
        var employees = routes.MapGroup("api/employees").WithTags("Employees");

        // GET /api/employees - List employees with pagination
        employees.MapGet("/", Queries.GetEmployees);

        // GET /api/employees/{id} - Get employee by ID
        employees.MapGet("/{id:guid}", Queries.GetEmployeeById);

        // POST /api/employees - Create new employee
        employees.MapPost("/", Commands.CreateEmployee);
    }

    private static class Commands
    {
        public static async Task<Results<CreatedAtRoute<CreateEmployeeResponse>, BadRequest<Error>>> CreateEmployee(
            [FromServices] ISender sender,
            [FromBody] CreateEmployeeRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new CreateEmployeeCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                request.Role,
                request.HourlyRate,
                request.Specialties,
                request.CanPerformInspections,
                request.InspectionLicenseNumber
            );

            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.CreatedAtRoute(result.Value!, "GetEmployeeById", new { id = result.Value!.Id })
                : TypedResults.BadRequest(result.Error!);
        }
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetEmployeesResponse>, BadRequest<Error>>> GetEmployees(
            [FromServices] ISender sender,
            int pageNumber = 1,
            int pageSize = 10,
            EmployeeRole? role = null,
            bool? isActive = null,
            string? searchTerm = null)
        {
            var query = new GetEmployeesQuery(pageNumber, pageSize, role, isActive, searchTerm);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<EmployeeResponse>, NotFound<Error>>> GetEmployeeById(
            [FromServices] ISender sender,
            Guid id)
        {
            var query = new GetEmployeeByIdQuery(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.NotFound(result.Error!);
        }
    }
}

// DTOs for Employee Endpoints
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

