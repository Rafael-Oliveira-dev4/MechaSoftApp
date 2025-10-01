using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Customers.Commands.ToggleCustomerActive;

public class ToggleCustomerActiveCommandHandler : IRequestHandler<ToggleCustomerActiveCommand, Result<ToggleCustomerActiveResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ToggleCustomerActiveCommandHandler> _logger;

    public ToggleCustomerActiveCommandHandler(IUnitOfWork unitOfWork, ILogger<ToggleCustomerActiveCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ToggleCustomerActiveResponse, Success, Error>> Handle(ToggleCustomerActiveCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            _logger.LogWarning("Attempt to toggle active status for non-existent customer: {CustomerId}", request.CustomerId);
            return Error.CustomerNotFound;
        }

        var oldStatus = customer.IsActive;
        
        // Soft-delete: just toggle IsActive flag
        // Customer data is preserved in database for historical purposes
        customer.IsActive = request.IsActive;

        await _unitOfWork.CustomerRepository.UpdateAsync(customer);
        await _unitOfWork.CommitAsync(cancellationToken);

        var action = request.IsActive ? "reativado" : "desativado";
        var message = $"Cliente {action} com sucesso";
        
        if (!string.IsNullOrWhiteSpace(request.Reason))
            message += $". Motivo: {request.Reason}";

        _logger.LogInformation("Customer {Action}: {CustomerId}, {CustomerName}. Reason: {Reason}", 
            action, customer.Id, customer.Name.FullName, request.Reason ?? "N/A");

        var response = new ToggleCustomerActiveResponse(
            customer.Id,
            customer.Name.FullName,
            customer.IsActive,
            message
        );

        return response;
    }
}

