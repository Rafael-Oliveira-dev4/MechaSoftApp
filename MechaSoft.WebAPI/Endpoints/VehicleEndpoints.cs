using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.Vehicles.Commands.CreateVehicle;
using MechaSoft.Application.CQ.Vehicles.Queries.GetVehicles;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class VehicleEndpoints
{
    public static void RegisterVehicleEndpoints(this IEndpointRouteBuilder routes)
    {
        var vehicles = routes.MapGroup("api/vehicles");

        // GET /api/vehicles - Listar veículos com paginação
        vehicles.MapGet("/", Queries.GetVehicles)
            .WithName("GetVehicles")
            .Produces<GetVehiclesResponse>(200)
            .Produces<Error>(400);

        // GET /api/vehicles/{id} - Obter veículo por ID
        vehicles.MapGet("/{id:guid}", Queries.GetVehicleById)
            .WithName("GetVehicleById")
            .Produces<VehicleResponse>(200)
            .Produces<Error>(404);

        // GET /api/vehicles/customer/{customerId} - Obter veículos de um cliente
        vehicles.MapGet("/customer/{customerId:guid}", Queries.GetVehiclesByCustomer)
            .WithName("GetVehiclesByCustomer")
            .Produces<GetVehiclesResponse>(200)
            .Produces<Error>(400);

        // POST /api/vehicles - Criar novo veículo
        vehicles.MapPost("/", Queries.CreateVehicle)
            .WithName("CreateVehicle")
            .Produces<CreateVehicleResponse>(201)
            .Produces<Error>(400);
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetVehiclesResponse>, BadRequest<Error>>> GetVehicles(
            [FromServices] ISender sender,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? customerId = null,
            [FromQuery] string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVehiclesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                CustomerId = customerId,
                SearchTerm = searchTerm
            };

            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }

        public static async Task<Results<Ok<VehicleResponse>, NotFound<Error>>> GetVehicleById(
            [FromServices] ISender sender,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            // TODO: Implement GetVehicleByIdQuery
            return TypedResults.NotFound(Error.VehicleNotFound);
        }

        public static async Task<Results<Ok<GetVehiclesResponse>, BadRequest<Error>>> GetVehiclesByCustomer(
            [FromServices] ISender sender,
            [FromRoute] Guid customerId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVehiclesQuery
            {
                PageNumber = 1,
                PageSize = 100,
                CustomerId = customerId,
                SearchTerm = null
            };

            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }

        public static async Task<Results<CreatedAtRoute<CreateVehicleResponse>, BadRequest<Error>>> CreateVehicle(
            [FromServices] ISender sender,
            [FromBody] CreateVehicleCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(command, cancellationToken);
            
            return result.IsSuccess
                ? TypedResults.CreatedAtRoute(result.Value!, "GetVehicleById", new { id = result.Value!.Id })
                : TypedResults.BadRequest(result.Error!);
        }
    }
}