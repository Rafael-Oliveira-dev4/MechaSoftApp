using MechaSoft.Application.CQ.Services.Commands.CreateService;
using MechaSoft.Application.CQ.Services.Queries.GetServiceById;
using MechaSoft.Application.CQ.Services.Queries.GetServices;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MechaSoft.Application.CQ.Services.Common;

namespace MechaSoft.WebAPI.Endpoints;

public static class ServiceEndpoints
{
    public static void RegisterServiceEndpoints(this IEndpointRouteBuilder routes)
    {
        var services = routes.MapGroup("api/services").WithTags("Services");

        // GET /api/services - List services with pagination
        services.MapGet("/", Queries.GetServices)
                .WithName("GetServices");

        // GET /api/services/{id} - Get service by ID
        services.MapGet("/{id:guid}", Queries.GetServiceById)
                .WithName("GetServiceById");

        // POST /api/services - Create new service
        services.MapPost("/", Commands.CreateService);
    }

    private static class Commands
    {
        public static async Task<Results<CreatedAtRoute<CreateServiceResponse>, BadRequest<Error>>> CreateService(
            [FromServices] ISender sender,
            [FromBody] CreateServiceRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new CreateServiceCommand(
                request.Name,
                request.Description,
                request.Category,
                request.EstimatedHours,
                request.PricePerHour,
                request.FixedPrice,
                request.RequiresInspection
            );

            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.CreatedAtRoute(result.Value!, "GetServiceById", new { id = result.Value!.Id })
                : TypedResults.BadRequest(result.Error!);
        }
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetServicesResponse>, BadRequest<Error>>> GetServices(
            [FromServices] ISender sender,
            int pageNumber = 1,
            int pageSize = 10,
            ServiceCategory? category = null,
            bool? isActive = null,
            string? searchTerm = null)
        {
            var query = new GetServicesQuery(pageNumber, pageSize, category, isActive, searchTerm);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<ServiceResponse>, NotFound<Error>>> GetServiceById(
            [FromServices] ISender sender,
            Guid id)
        {
            var query = new GetServiceByIdQuery(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.NotFound(result.Error!);
        }
    }
}

