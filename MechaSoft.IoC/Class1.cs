using MechaSoft.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MechaSoft.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddIoCServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataServices(configuration);
        services.AddApplicationServices(configuration);

        //DreamLuso.Security 

        return services;
    }
}