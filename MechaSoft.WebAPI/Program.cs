using MechaSoft.IoC;
using MechaSoft.WebAPI.Middleware;
using MechaSoft.WebAPI.Endpoints;
using MechaSoft.WebAPI.Extensions;
using MechaSoft.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIoCServices(builder.Configuration);
builder.Services.AddSecurityServices(builder.Configuration);

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicionar middleware de tratamento global de erros
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Mapear endpoints organizados por módulo
app.RegisterEndpoints();

app.Run();
