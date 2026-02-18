using MechaSoft.Data.Context;
using MechaSoft.IoC;
using MechaSoft.WebAPI.Middleware;
using MechaSoft.WebAPI.Endpoints;
using MechaSoft.WebAPI.Extensions;
using MechaSoft.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Evita conflitos de nomes de schemas (ex.: VehicleResponse duplicado em queries diferentes)
    c.CustomSchemaIds(type => type.FullName);
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// CORS Configuration
var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddIoCServices(builder.Configuration, builder.Environment);
builder.Services.AddSecurityServices(builder.Configuration);

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});
builder.Services.AddAuthorization();

// Health checks: aplicação + base de dados (EF Core DbContext)
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(name: "database");

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicionar middleware de tratamento global de erros
app.UseMiddleware<GlobalExceptionMiddleware>();

// CORS (deve vir antes de UseHttpsRedirection para que respostas tenham os headers)
app.UseCors("AllowAngularApp");

// Static Files (for serving uploaded images)
app.UseStaticFiles();

// Em desenvolvimento, não redirecionar HTTP→HTTPS para evitar CORS no redirect (Angular em http://localhost:4200)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Mapear endpoints organizados por módulo
app.RegisterEndpoints();

// Health Check Endpoint (devolve JSON com status e descrição dos checks)
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = report.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy
            ? 200
            : 503;
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            entries = report.Entries.ToDictionary(e => e.Key, e => new
            {
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        }, new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(result);
    }
});

app.Run();
