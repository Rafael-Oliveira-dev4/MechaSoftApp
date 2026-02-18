using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MechaSoft.Security.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<CreateEmployeeResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher,
        ILogger<CreateEmployeeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<CreateEmployeeResponse, Success, Error>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        var existingEmployee = await _unitOfWork.EmployeeRepository.GetByEmailAsync(request.Email);
        if (existingEmployee != null)
        {
            _logger.LogWarning("Attempt to create employee with existing email: {Email}", request.Email);
            return Error.ExistingEmployee;
        }

        // Create hourly rate Money object if provided
        Money? hourlyRate = request.HourlyRate.HasValue 
            ? new Money(request.HourlyRate.Value, "EUR") 
            : null;

        // Create employee
        var employee = new Employee(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.Role,
            hourlyRate
        );

        // Set specialties if provided
        if (request.Specialties != null && request.Specialties.Any())
        {
            foreach (var specialty in request.Specialties)
            {
                employee.AddSpecialty(specialty);
            }
        }

        // Set inspection details
        employee.CanPerformInspections = request.CanPerformInspections;
        employee.InspectionLicenseNumber = request.InspectionLicenseNumber;

        // Save employee
        var savedEmployee = await _unitOfWork.EmployeeRepository.SaveAsync(employee);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Employee created successfully: {EmployeeId}, {EmployeeName}", 
            savedEmployee.Id, savedEmployee.Name.FullName);

        // AUTO-CREATE USER ACCOUNT LINKED TO EMPLOYEE
        string? generatedUsername = null;
        string? generatedPassword = null;

        try
        {
            // Generate username from email (e.g., joao.silva@email.com -> joao.silva)
            generatedUsername = request.Email.Split('@')[0];

            // Check if username already exists
            var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(generatedUsername);
            if (existingUser != null)
            {
                // Append employee ID to make it unique
                generatedUsername = $"{generatedUsername}_{savedEmployee.Id.ToString().Substring(0, 4)}";
            }

            // Generate temporary password
            generatedPassword = GenerateTemporaryPassword();

            // Hash password
            var hashedPassword = _passwordHasher.HashPassword(generatedPassword);

            // Determine UserRole based on EmployeeRole
            var userRole = request.Role == EmployeeRole.Manager || request.Role == EmployeeRole.Owner
                ? UserRole.Admin
                : UserRole.Employee;

            // Create User account using object initializer
            var user = new User
            {
                Username = generatedUsername,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Salt = null, // BCrypt doesn't use separate salt
                Role = userRole,
                IsActive = true,
                EmailConfirmed = false
            };

            // Link User to Employee
            user.LinkToEmployee(savedEmployee.Id);

            // Save user
            await _unitOfWork.UserRepository.SaveAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "User account created and linked to Employee: Username={Username}, EmployeeId={EmployeeId}", 
            generatedUsername, 
            savedEmployee.Id);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, 
            "❌ FAILED to auto-create user account for Employee {EmployeeId}. User can be created manually later. Exception: {ExceptionMessage}", 
            savedEmployee.Id, 
            ex.Message);
        // Don't fail the entire operation if user creation fails
        // Employee is already created successfully
        // Reset credentials to null on failure
        generatedUsername = null;
        generatedPassword = null;
    }

    _logger.LogInformation(
        "🔍 DEBUG: Returning CreateEmployeeResponse with GeneratedUsername={Username}, GeneratedPassword={PasswordExists}", 
        generatedUsername, 
        generatedPassword != null ? "***SET***" : "NULL");

        var response = new CreateEmployeeResponse(
            savedEmployee.Id,
            savedEmployee.Name.FullName,
            savedEmployee.Email,
            savedEmployee.Phone,
            savedEmployee.Role,
            savedEmployee.CreatedAt ?? DateTime.UtcNow,
            generatedUsername,
            generatedPassword  // Only returned once - frontend should show this to admin
        );

        return response;
    }

    /// <summary>
    /// Generates a secure temporary password
    /// Format: Temp@XXXX where XXXX is a random 4-digit number
    /// </summary>
    private static string GenerateTemporaryPassword()
    {
        var random = new Random();
        var randomNumber = random.Next(1000, 9999);
        return $"Temp@{randomNumber}";
    }
}

