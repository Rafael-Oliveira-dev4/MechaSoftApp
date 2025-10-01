using MechaSoft.Application.CQ.Parts.Commands.CreatePart;
using MechaSoft.Application.CQ.Parts.Commands.UpdateStock;
using MechaSoft.Application.CQ.Parts.Queries.GetPartById;
using MechaSoft.Application.CQ.Parts.Queries.GetParts;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class PartEndpoints
{
    public static void RegisterPartEndpoints(this IEndpointRouteBuilder routes)
    {
        var parts = routes.MapGroup("api/parts").WithTags("Parts");

        // GET /api/parts - List parts with pagination
        parts.MapGet("/", Queries.GetParts);

        // GET /api/parts/{id} - Get part by ID
        parts.MapGet("/{id:guid}", Queries.GetPartById);

        // POST /api/parts - Create new part
        parts.MapPost("/", Commands.CreatePart);

        // PUT /api/parts/{id}/stock - Update part stock
        parts.MapPut("/{id:guid}/stock", Commands.UpdateStock);
    }

    private static class Commands
    {
        public static async Task<Results<CreatedAtRoute<CreatePartResponse>, BadRequest<Error>>> CreatePart(
            [FromServices] ISender sender,
            [FromBody] CreatePartRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new CreatePartCommand(
                request.Code,
                request.Name,
                request.Description,
                request.Category,
                request.Brand,
                request.UnitCost,
                request.SalePrice,
                request.StockQuantity,
                request.MinStockLevel,
                request.SupplierName,
                request.SupplierContact,
                request.Location
            );

            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.CreatedAtRoute(result.Value!, "GetPartById", new { id = result.Value!.Id })
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<UpdateStockResponse>, BadRequest<Error>>> UpdateStock(
            [FromServices] ISender sender,
            Guid id,
            [FromBody] UpdateStockRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new UpdateStockCommand(id, request.Quantity, request.MovementType, request.Reason);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetPartsResponse>, BadRequest<Error>>> GetParts(
            [FromServices] ISender sender,
            int pageNumber = 1,
            int pageSize = 10,
            string? category = null,
            bool? isActive = null,
            bool? lowStockOnly = null,
            string? searchTerm = null)
        {
            var query = new GetPartsQuery(pageNumber, pageSize, category, isActive, lowStockOnly, searchTerm);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<PartResponse>, NotFound<Error>>> GetPartById(
            [FromServices] ISender sender,
            Guid id)
        {
            var query = new GetPartByIdQuery(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.NotFound(result.Error!);
        }
    }
}

// DTOs for Part Endpoints
public record CreatePartRequest(
    string Code,
    string Name,
    string Description,
    string Category,
    string? Brand,
    decimal UnitCost,
    decimal SalePrice,
    int StockQuantity,
    int MinStockLevel,
    string? SupplierName,
    string? SupplierContact,
    string? Location
);

public record UpdateStockRequest(
    int Quantity,
    StockMovementType MovementType,
    string? Reason
);

