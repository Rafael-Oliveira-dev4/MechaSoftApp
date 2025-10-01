using MechaSoft.Data.Context;
using MechaSoft.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Core.Interfaces;
using MechaSoft.Data.Repositories;
using MechaSoft.Data.Uow;

namespace MechaSoft.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Infra - HttpContext for interceptors/auditing
        services.AddHttpContextAccessor();

        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("MechaSoftCS") 
            ?? throw new InvalidOperationException("Connection string 'MechaSoftCS' not found.");

        // Interceptors and Context
        // Re-enabled with optional IHttpContextAccessor
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                // Configure SQL Server options
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(30);
            });
        });

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // Adicione outros conversores se necessário
            });

        // Repository
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInspectionRepository, InspectionRepository>();
        services.AddScoped<IPartRepository, PartRepository>();
        services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        //Use Cases
        services.AddScoped<ICreateServiceOrderUseCase, CreateServiceOrderUseCaseRepository>();
        services.AddScoped<IScheduleInspectionUseCase, ScheduleInspectionRepository>();

        // Note: ITokenService is registered in MechaSoft.Security project
        // services.AddScoped<IFileStorageService, FileStorageService>();
        //services.AddScoped<INotificationService, NotificationService>();

        // Health Checks (registered in WebAPI Program)
        services.AddHealthChecks();

        return services;
    }
}