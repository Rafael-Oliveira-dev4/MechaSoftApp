using MechaSoft.Data.Context;
using MechaSoft.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace MechaSoft.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Interceptors and Context
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DV_RO_MechaSoft;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True", sqlOptions =>
            {
                // Configure SQL Server options if needed
                sqlOptions.EnableRetryOnFailure(); // Example: Enable retry on failure
            });
        });

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // Adicione outros conversores se necessário
            });

        // Repository
        //services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddScoped<ICustomerRepository, CustomerRepository>();
        //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        //services.AddScoped<IInspectionRepository, InspectionRepository>();
        //services.AddScoped<IPartRepository, PartRepository>();
        //services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
        //services.AddScoped<IServiceRepository, ServiceRepository>();
        //services.AddScoped<IVehicleRepository, VehicleRepository>();

        // Use Cases
        //services.AddScoped<ICreateServiceOrderUseCase, CreateServiceOrderUseCase>();
        //services.AddScoped<IScheduleInspectionUseCase, ScheduleInspectionUseCase>();

        // Services (adicione conforme necessário)
        // services.AddScoped<IFileStorageService, FileStorageService>();
        // services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}