using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Customers.Commands.CompleteCustomerProfile;

public class CompleteCustomerProfileCommandHandler 
    : IRequestHandler<CompleteCustomerProfileCommand, Result<CompleteCustomerProfileResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteCustomerProfileCommandHandler> _logger;

    public CompleteCustomerProfileCommandHandler(
        IUnitOfWork unitOfWork, 
        ILogger<CompleteCustomerProfileCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CompleteCustomerProfileResponse, Success, Error>> Handle(
        CompleteCustomerProfileCommand request, 
        CancellationToken cancellationToken)
    {
        // Get user
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", request.UserId);
            return Error.UserNotFound;
        }

        // Check if user already has a customer profile
        if (user.CustomerId.HasValue)
        {
            _logger.LogWarning("User already has a customer profile: {UserId}, {CustomerId}", 
                request.UserId, user.CustomerId);
            return Error.CustomerAlreadyExists;
        }

        // Check if user role is Customer
        if (user.Role != UserRole.Customer)
        {
            _logger.LogWarning("User is not a customer: {UserId}, Role: {Role}", 
                request.UserId, user.Role);
            return Error.InvalidOperation;
        }

        // Note: Email uniqueness is already guaranteed by unique constraint in database

        // Check if NIF is provided and already exists
        if (!string.IsNullOrWhiteSpace(request.Nif))
        {
            var existingCustomerByNif = await _unitOfWork.CustomerRepository.GetByNifAsync(request.Nif);
            if (existingCustomerByNif != null)
            {
                _logger.LogWarning("Customer with NIF already exists: {Nif}", request.Nif);
                return Error.ExistingNif;
            }
        }

        try
        {
            // Create address
            var address = new Address(
                request.Street,
                request.Number,
                request.Parish,
                request.Municipality,
                request.District,
                request.PostalCode,
                request.Complement
            );

            // Create customer using constructor
            var customer = new Customer(
                request.FirstName,
                request.LastName,
                user.Email, // Use email from user
                request.Phone,
                address,
                request.Type,
                request.Nif,
                request.CitizenCard
            );

            // Save customer
            var savedCustomer = await _unitOfWork.CustomerRepository.SaveAsync(customer);
            
            // Link customer to user
            user.LinkToCustomer(savedCustomer.Id);
            await _unitOfWork.UserRepository.UpdateAsync(user);
            
            // Commit transaction
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Customer profile completed successfully: UserId={UserId}, CustomerId={CustomerId}, Name={CustomerName}", 
                user.Id, savedCustomer.Id, savedCustomer.Name.FullName);

            var response = new CompleteCustomerProfileResponse
            {
                CustomerId = savedCustomer.Id,
                FullName = savedCustomer.Name.FullName,
                Email = savedCustomer.Email,
                Phone = savedCustomer.Phone,
                Type = savedCustomer.Type,
                Nif = savedCustomer.Nif,
                CreatedAt = DateTime.UtcNow
            };

            return response;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error while completing customer profile: {UserId}, Message: {Message}", 
                request.UserId, ex.Message);
            return Error.ValidationFailed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing customer profile: {UserId}, Message: {Message}", 
                request.UserId, ex.Message);
            return Error.ServerError;
        }
    }
}

