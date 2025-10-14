using MechaSoft.Application.CQ.ServiceOrders.Commands.AddPartToOrder;
using MechaSoft.Application.CQ.ServiceOrders.Commands.AddServiceToOrder;
using MechaSoft.Application.CQ.ServiceOrders.Commands.AssignMechanic;
using MechaSoft.Application.CQ.ServiceOrders.Commands.CreateServiceOrder;
using MechaSoft.Application.CQ.ServiceOrders.Commands.UpdateServiceOrderStatus;
using MechaSoft.Application.CQ.ServiceOrders.Queries.GetServiceOrderById;
using MechaSoft.Application.CQ.ServiceOrders.Queries.GetServiceOrders;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Application.CQ.ServiceOrders.Common;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class ServiceOrderEndpoints
{
    public static void RegisterServiceOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var serviceOrders = routes.MapGroup("api/service-orders").WithTags("Service Orders");

        // GET /api/service-orders - Listar ordens de serviço com paginação
        serviceOrders.MapGet("/", Queries.GetServiceOrders);

        // GET /api/service-orders/{id} - Obter ordem de serviço por ID
        serviceOrders.MapGet("/{id:guid}", Queries.GetServiceOrderById)
                      .WithName("GetServiceOrderById");

        // GET /api/service-orders/customer/{customerId} - Obter ordens de um cliente
        serviceOrders.MapGet("/customer/{customerId:guid}", Queries.GetServiceOrdersByCustomer);

        // POST /api/service-orders - Create new service order
        serviceOrders.MapPost("/", Commands.CreateServiceOrder);

        // PUT /api/service-orders/{id}/status - Update order status
        serviceOrders.MapPut("/{id:guid}/status", Commands.UpdateStatus);

        // PUT /api/service-orders/{id}/mechanic - Assign mechanic
        serviceOrders.MapPut("/{id:guid}/mechanic", Commands.AssignMechanic);

        // POST /api/service-orders/{id}/services - Add service to order
        serviceOrders.MapPost("/{id:guid}/services", Commands.AddService);

        // POST /api/service-orders/{id}/parts - Add part to order
        serviceOrders.MapPost("/{id:guid}/parts", Commands.AddPart);
    }

    private static class Commands
    {
        public static async Task<Results<CreatedAtRoute<CreateServiceOrderResponse>, BadRequest<Error>>> CreateServiceOrder(
            [FromServices] ISender sender,
            [FromBody] MechaSoft.Application.CQ.ServiceOrders.Common.CreateServiceOrderRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new CreateServiceOrderCommand(
                request.CustomerId,
                request.VehicleId,
                request.Description,
                request.Priority,
                request.EstimatedCost,
                request.EstimatedDelivery,
                request.MechanicId,
                request.RequiresInspection,
                request.InternalNotes
            );

            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.CreatedAtRoute(result.Value!, "GetServiceOrderById", new { id = result.Value!.Id })
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<UpdateServiceOrderStatusResponse>, BadRequest<Error>>> UpdateStatus(
            [FromServices] ISender sender,
            Guid id,
            [FromBody] UpdateStatusRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new UpdateServiceOrderStatusCommand(id, request.Status, request.Notes);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<AssignMechanicResponse>, BadRequest<Error>>> AssignMechanic(
            [FromServices] ISender sender,
            Guid id,
            [FromBody] AssignMechanicRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new AssignMechanicCommand(id, request.MechanicId);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<AddServiceToOrderResponse>, BadRequest<Error>>> AddService(
            [FromServices] ISender sender,
            Guid id,
            [FromBody] AddServiceRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new AddServiceToOrderCommand(
                id,
                request.ServiceId,
                request.Quantity,
                request.EstimatedHours,
                request.DiscountPercentage,
                request.MechanicId
            );

            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<AddPartToOrderResponse>, BadRequest<Error>>> AddPart(
            [FromServices] ISender sender,
            Guid id,
            [FromBody] AddPartRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new AddPartToOrderCommand(id, request.PartId, request.Quantity, request.DiscountPercentage);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetServiceOrdersResponse>, BadRequest<Error>>> GetServiceOrders(
            [FromServices] ISender sender,
            int pageNumber = 1,
            int pageSize = 10,
            ServiceOrderStatus? status = null)
        {
            var query = new GetServiceOrdersQuery(pageNumber, pageSize, null, status);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<ServiceOrderResponse>, NotFound<Error>>> GetServiceOrderById(
            [FromServices] ISender sender,
            Guid id)
        {
            var query = new GetServiceOrderByIdQuery(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.NotFound(result.Error!);
        }

        public static async Task<Results<Ok<GetServiceOrdersResponse>, BadRequest<Error>>> GetServiceOrdersByCustomer(
            [FromServices] ISender sender,
            Guid customerId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = new GetServiceOrdersQuery(pageNumber, pageSize, customerId, null);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }
    }
}