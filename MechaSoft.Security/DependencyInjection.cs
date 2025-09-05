using MechaSoft.Security.Interfaces;
using MechaSoft.Security.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MechaSoft.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
