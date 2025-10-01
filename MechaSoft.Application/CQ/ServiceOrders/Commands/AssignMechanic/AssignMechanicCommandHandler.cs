using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.ServiceOrders.Commands.AssignMechanic;

public class AssignMechanicCommandHandler : IRequestHandler<AssignMechanicCommand, Result<AssignMechanicResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignMechanicCommandHandler> _logger;

    public AssignMechanicCommandHandler(IUnitOfWork unitOfWork, ILogger<AssignMechanicCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<AssignMechanicResponse, Success, Error>> Handle(AssignMechanicCommand request, CancellationToken cancellationToken)
    {
        var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.ServiceOrderId);
        if (serviceOrder == null)
        {
            _logger.LogWarning("Attempt to assign mechanic to non-existent service order: {ServiceOrderId}", request.ServiceOrderId);
            return Error.ServiceOrderNotFound;
        }

        var mechanic = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.MechanicId);
        if (mechanic == null)
        {
            _logger.LogWarning("Attempt to assign non-existent mechanic: {MechanicId}", request.MechanicId);
            return Error.EmployeeNotFound;
        }

        // Verify employee is a mechanic or can work on orders
        if (mechanic.Role != Domain.Model.EmployeeRole.Mechanic && 
            mechanic.Role != Domain.Model.EmployeeRole.Owner)
        {
            _logger.LogWarning("Attempt to assign non-mechanic employee to service order: {EmployeeId}, Role: {Role}", 
                mechanic.Id, mechanic.Role);
            return new Error("InvalidEmployeeRole", "Only mechanics and owners can be assigned to service orders");
        }

        if (!mechanic.IsActive)
        {
            _logger.LogWarning("Attempt to assign inactive mechanic: {MechanicId}", request.MechanicId);
            return new Error("InactiveMechanic", "Cannot assign inactive mechanic to service order");
        }

        serviceOrder.MechanicId = request.MechanicId;

        await _unitOfWork.ServiceOrderRepository.UpdateAsync(serviceOrder);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Mechanic assigned to service order: {OrderNumber}, Mechanic: {MechanicName}", 
            serviceOrder.OrderNumber, mechanic.Name.FullName);

        var response = new AssignMechanicResponse(
            serviceOrder.Id,
            serviceOrder.OrderNumber,
            mechanic.Id,
            mechanic.Name.FullName
        );

        return response;
    }
}
