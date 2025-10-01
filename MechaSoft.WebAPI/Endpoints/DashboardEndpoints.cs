using MechaSoft.Application.CQ.Dashboard.Queries.GetDashboardStats;
using MechaSoft.Application.CQ.Reports.Queries.GetLowStockReport;
using MechaSoft.Application.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.WebAPI.Endpoints;

public static class DashboardEndpoints
{
    public static void RegisterDashboardEndpoints(this IEndpointRouteBuilder routes)
    {
        var dashboard = routes.MapGroup("api/dashboard").WithTags("Dashboard & Reports");

        // GET /api/dashboard/stats - Dashboard overview statistics
        dashboard.MapGet("/stats", Queries.GetDashboardStats);

        // GET /api/dashboard/reports/low-stock - Low stock report
        dashboard.MapGet("/reports/low-stock", Queries.GetLowStockReport);
    }

    private static class Queries
    {
        public static async Task<Results<Ok<DashboardStatsResponse>, BadRequest<Error>>> GetDashboardStats(
            [FromServices] ISender sender)
        {
            var query = new GetDashboardStatsQuery();
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }

        public static async Task<Results<Ok<LowStockReportResponse>, BadRequest<Error>>> GetLowStockReport(
            [FromServices] ISender sender)
        {
            var query = new GetLowStockReportQuery();
            var result = await sender.Send(query);

            return result.IsSuccess
                ? TypedResults.Ok(result.Value!)
                : TypedResults.BadRequest(result.Error!);
        }
    }
}

