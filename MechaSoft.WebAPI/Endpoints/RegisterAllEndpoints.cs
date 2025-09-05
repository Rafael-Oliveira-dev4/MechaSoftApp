namespace MechaSoft.WebAPI.Endpoints;

public static class RegisterAllEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.RegisterAccountEndpoints();
        routes.RegisterCustomerEndpoints();
        routes.RegisterVehicleEndpoints();
        routes.RegisterServiceOrderEndpoints();
    }
}
