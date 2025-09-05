namespace MechaSoft.Application.Common.Responses;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NotImplemented = new("NotImplemented", "Method not implemented");
    public static readonly Error EmptyDatabase = new("EmptyDatabase", "No data found");
    public static readonly Error ExistingUser = new("ExistingUser", "There is a user with this email");
    public static readonly Error InvalidInput = new("InvalidInput", "The input provided is invalid");
    public static readonly Error UnauthorizedAccess = new("UnauthorizedAccess", "User is not authorized to perform this action");
    public static readonly Error OperationFailed = new("OperationFailed", "The operation failed to complete");
    public static readonly Error Timeout = new("Timeout", "The operation timed out");
    public static readonly Error ServiceUnavailable = new("ServiceUnavailable", "The service is currently unavailable");
    public static readonly Error DatabaseError = new("DatabaseError", "An error occurred while accessing the database");
    public static readonly Error InvalidCredentials = new("InvalidCredentials", "The provided credentials are invalid");
    public static readonly Error UserLockedOut = new("UserLockedOut", "The user account is locked out");
    public static readonly Error PasswordExpired = new("PasswordExpired", "The user password has expired");
    public static readonly Error ConcurrencyConflict = new("ConcurrencyConflict", "A concurrency conflict occurred");
    public static readonly Error ResourceConflict = new("ResourceConflict", "A resource conflict occurred");
    public static readonly Error InsufficientPermissions = new("InsufficientPermissions", "Insufficient permissions to perform this action");
    public static readonly Error InvalidToken = new("InvalidToken", "The provided token is invalid or expired");
    public static readonly Error NotFound = new("NotFound", "The requested resource was not found");
    public static readonly Error ExternalServiceError = new("ExternalServiceError", "An error occurred in an external service");
    public static readonly Error BadRequest = new("BadRequest", "The request was invalid or cannot be served");
    public static readonly Error Conflict = new("Conflict", "The request could not be completed due to a conflict");

    // Account specific errors
    public static readonly Error UserNotFound = new("UserNotFound", "User Not Found");
    public static readonly Error AccountNotFound = new("AccountNotFound", "Account Not Found");
    public static readonly Error InvalidPassword = new("InvalidPassword", "Invalid Password");
    public static readonly Error UsernameAlreadyExists = new("UsernameAlreadyExists", "Username already exists");
    public static readonly Error EmailAlreadyExists = new("EmailAlreadyExists", "Email already exists");
    public static readonly Error AccountInactive = new("AccountInactive", "Account is inactive");
    public static readonly Error RefreshTokenExpired = new("RefreshTokenExpired", "Refresh token has expired");
    public static readonly Error InvalidRefreshToken = new("InvalidRefreshToken", "Invalid refresh token");
    public static readonly Error ResetTokenExpired = new("ResetTokenExpired", "Reset token has expired");
    public static readonly Error InvalidResetToken = new("InvalidResetToken", "Invalid reset token");
    public static readonly Error PasswordTooWeak = new("PasswordTooWeak", "Password does not meet security requirements");
    public static readonly Error PasswordsDoNotMatch = new("PasswordsDoNotMatch", "Passwords do not match");

    // Customer specific errors
    public static readonly Error CustomerNotFound = new("CustomerNotFound", "Customer Not Found");
    public static readonly Error ExistingCustomer = new("ExistingCustomer", "Customer already exists");

    // Employee specific errors
    public static readonly Error EmployeeNotFound = new("EmployeeNotFound", "Employee Not Found");
    public static readonly Error ExistingEmployee = new("ExistingEmployee", "Employee already exists");

    // Vehicle specific errors
    public static readonly Error VehicleNotFound = new("VehicleNotFound", "Vehicle Not Found");
    public static readonly Error ExistingVehicle = new("ExistingVehicle", "Vehicle already exists");

    // Service Order specific errors
    public static readonly Error ServiceOrderNotFound = new("ServiceOrderNotFound", "Service Order Not Found");
    public static readonly Error InvalidServiceOrder = new("InvalidServiceOrder", "Invalid Service Order");

    // Part specific errors
    public static readonly Error PartNotFound = new("PartNotFound", "Part Not Found");
    public static readonly Error InsufficientStock = new("InsufficientStock", "Insufficient stock available");

    // Service specific errors
    public static readonly Error ServiceNotFound = new("ServiceNotFound", "Service Not Found");
    public static readonly Error ExistingService = new("ExistingService", "Service already exists");

    // Inspection specific errors
    public static readonly Error InspectionNotFound = new("InspectionNotFound", "Inspection Not Found");
    public static readonly Error InvalidInspection = new("InvalidInspection", "Invalid Inspection");
}