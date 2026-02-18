using MechaSoft.Application;
using MechaSoft.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace MechaSoft.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddIoCServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDataServices(configuration);
        services.AddApplicationServices(configuration, environment);

        //DreamLuso.Security 

        return services;
    }
}