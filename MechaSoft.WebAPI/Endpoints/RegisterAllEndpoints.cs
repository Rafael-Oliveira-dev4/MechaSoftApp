namespace MechaSoft.WebAPI.Endpoints;

public static class RegisterAllEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routes)
    {
        // Account management
        routes.RegisterAccountEndpoints();
        
        // Core entities
        routes.RegisterCustomerEndpoints();
        routes.RegisterVehicleEndpoints();
        routes.RegisterEmployeeEndpoints();
        
        // Services and Parts
        routes.RegisterServiceEndpoints();
        routes.RegisterPartEndpoints();
        
        // Service Orders and Inspections
        routes.RegisterServiceOrderEndpoints();
        routes.RegisterInspectionEndpoints();
        
        // Dashboard and Reports
        routes.RegisterDashboardEndpoints();
    }
}
