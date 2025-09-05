using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.Customers.Commands.CreateCustomer;
using MechaSoft.Application.CQ.Customers.Queries.GetCustomers;
using MechaSoft.Application.CQ.Customers.Queries.GetCustomerById;
using MechaSoft.Application.CQ.Customers.Common;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class CustomerEndpoints
{
    public static void RegisterCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var customers = routes.MapGroup("api/customers");

        // GET /api/customers - Listar clientes com paginação
        customers.MapGet("/", Queries.GetCustomers)
            .WithName("GetCustomers")
            .Produces<GetCustomersResponse>(200)
            .Produces<Error>(400);

        // GET /api/customers/{id} - Obter cliente por ID
        customers.MapGet("/{id:guid}", Queries.GetCustomerById)
            .WithName("GetCustomerById")
            .Produces<CustomerResponse>(200)
            .Produces<Error>(404);

        // POST /api/customers - Criar novo cliente
        customers.MapPost("/", Queries.CreateCustomer)
            .WithName("CreateCustomer")
            .Produces<CreateCustomerResponse>(201)
            .Produces<Error>(400);
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetCustomersResponse>, BadRequest<Error>>> GetCustomers(
            [FromServices] ISender sender,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetCustomersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm
            };

            var result = await sender.Send(query, cancellationToken);
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.BadRequest(result.Error);
        }

        public static async Task<Results<Ok<CustomerResponse>, NotFound<Error>>> GetCustomerById(
            [FromServices] ISender sender,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = new GetCustomerByIdQuery { Id = id };
            var result = await sender.Send(query, cancellationToken);
            
            return result.IsSuccess
                ? TypedResults.Ok(result.Value)
                : TypedResults.NotFound(result.Error);
        }

        public static async Task<Results<CreatedAtRoute<CreateCustomerResponse>, BadRequest<Error>>> CreateCustomer(
            [FromServices] ISender sender,
            [FromBody] CreateCustomerCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(command, cancellationToken);
            
            return result.IsSuccess
                ? TypedResults.CreatedAtRoute(result.Value!, "GetCustomerById", new { id = result.Value!.Id })
                : TypedResults.BadRequest(result.Error!);
        }
    }
}