using MechaSoft.Application.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class ServiceOrderEndpoints
{
    public static void RegisterServiceOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var serviceOrders = routes.MapGroup("api/service-orders");

        // GET /api/service-orders - Listar ordens de serviço com paginação
        serviceOrders.MapGet("/", Commands.GetServiceOrders);

        // GET /api/service-orders/{id} - Obter ordem de serviço por ID
        serviceOrders.MapGet("/{id:guid}", Commands.GetServiceOrderById);

        // GET /api/service-orders/customer/{customerId} - Obter ordens de um cliente
        serviceOrders.MapGet("/customer/{customerId:guid}", Commands.GetServiceOrdersByCustomer);

        // POST /api/service-orders - Criar nova ordem de serviço
        serviceOrders.MapPost("/", Commands.CreateServiceOrder);
    }

    private static class Commands
    {
        public static async Task<Results<Ok<object>, BadRequest<Error>>> GetServiceOrders(
            [FromServices] ISender sender,
            int pageNumber = 1,
            int pageSize = 10)
        {
            // TODO: Implement GetServiceOrdersQuery
            return TypedResults.Ok((object)new { Message = "Service orders endpoint - TODO: Implement with MediatR" });
        }

        public static async Task<Results<Ok<object>, NotFound<Error>>> GetServiceOrderById(
            [FromServices] ISender sender,
            Guid id)
        {
            // TODO: Implement GetServiceOrderByIdQuery
            return TypedResults.NotFound(Error.ServiceOrderNotFound);
        }

        public static async Task<Results<Ok<object>, BadRequest<Error>>> GetServiceOrdersByCustomer(
            [FromServices] ISender sender,
            Guid customerId)
        {
            // TODO: Implement GetServiceOrdersByCustomerQuery
            return TypedResults.Ok((object)new { Message = "Service orders by customer endpoint - TODO: Implement with MediatR" });
        }

        public static async Task<Results<CreatedAtRoute<object>, BadRequest<Error>>> CreateServiceOrder(
            [FromServices] ISender sender,
            [FromBody] CreateServiceOrderRequest request)
        {
            // TODO: Implement CreateServiceOrderCommand
            return TypedResults.BadRequest(Error.NotImplemented);
        }
    }
}

// DTOs para ServiceOrder Endpoints
public record CreateServiceOrderRequest(
    Guid CustomerId,
    Guid VehicleId,
    string Description,
    decimal EstimatedCost
);