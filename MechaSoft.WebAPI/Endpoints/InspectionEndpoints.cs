using MechaSoft.Application.CQ.Inspections.Commands.CreateInspection;
using MechaSoft.Application.CQ.Inspections.Commands.UpdateInspectionResult;
using MechaSoft.Application.CQ.Inspections.Queries.GetInspectionById;
using MechaSoft.Application.CQ.Inspections.Queries.GetInspections;
using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class InspectionEndpoints
{
    public static void RegisterInspectionEndpoints(this IEndpointRouteBuilder routes)
    {
        var inspections = routes.MapGroup("api/inspections").WithTags("Inspections");

        // GET /api/inspections - List inspections with pagination
        inspections.MapGet("/", Queries.GetInspections)
                    .WithName("GetInspections");

        // GET /api/inspections/{id} - Get inspection by ID
        inspections.MapGet("/{id:guid}", Queries.GetInspectionById)
                    .WithName("GetInspectionById");

        // POST /api/inspections - Create new inspection
        inspections.MapPost("/", Commands.CreateInspection);

        // PUT /api/inspections/{id}/result - Update inspection result
        inspections.MapPut("/{id:guid}/result", Commands.UpdateInspectionResult);
    }

    private static class Commands
    {
        public static async Task<Results<CreatedAtRoute<CreateInspectionResponse>, BadRequest<Error>>> CreateInspection(
            [FromServices] ISender sender,
            [FromBody] CreateInspectionRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new CreateInspectionCommand(
                request.VehicleId,
                request.ServiceOrderId,
                request.Type,
                request.InspectionDate,
                request.ExpiryDate,
                request.Cost,
                request.InspectionCenter,
                request.VehicleMileage,
                request.Observations
            );

            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.CreatedAtRoute(result.Value!, "GetInspectionById", new { id = result.Value!.Id })
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<UpdateInspectionResultResponse>, BadRequest<Error>>> UpdateInspectionResult(
            [FromServices] ISender sender,
            Guid id,
            [FromBody] UpdateInspectionResultRequest request)
        {
            if (request == null)
            {
                return TypedResults.BadRequest(Error.InvalidInput);
            }

            var command = new UpdateInspectionResultCommand(
                id,
                request.Result,
                request.CertificateNumber,
                request.Observations
            );

            var result = await sender.Send(command);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }
    }

    private static class Queries
    {
        public static async Task<Results<Ok<GetInspectionsResponse>, BadRequest<Error>>> GetInspections(
            [FromServices] ISender sender,
            int pageNumber = 1,
            int pageSize = 10,
            Guid? vehicleId = null,
            InspectionResult? result = null)
        {
            var query = new GetInspectionsQuery(pageNumber, pageSize, vehicleId, result);
            var queryResult = await sender.Send(query);

            return queryResult.IsSuccess
                ? TypedResults.Ok(queryResult.Value!)
                : TypedResults.BadRequest(queryResult.Error!);
        }

        public static async Task<Results<Ok<InspectionResponse>, NotFound<Error>>> GetInspectionById(
            [FromServices] ISender sender,
            Guid id)
        {
            var query = new GetInspectionByIdQuery(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.NotFound(result.Error!);
        }
    }
}

// DTOs for Inspection Endpoints
public record CreateInspectionRequest(
    Guid VehicleId,
    Guid ServiceOrderId,
    InspectionType Type,
    DateTime InspectionDate,
    DateTime ExpiryDate,
    decimal Cost,
    string InspectionCenter,
    int VehicleMileage,
    string? Observations
);

public record UpdateInspectionResultRequest(
    InspectionResult Result,
    string? CertificateNumber,
    string? Observations
);

