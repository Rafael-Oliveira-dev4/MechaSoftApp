using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Parts.Commands.UpdatePart;

public class UpdatePartCommandHandler : IRequestHandler<UpdatePartCommand, Result<UpdatePartResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePartCommandHandler> _logger;

    public UpdatePartCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePartCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UpdatePartResponse, Success, Error>> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.PartRepository.GetByIdAsync(request.Id);
        if (part == null)
        {
            _logger.LogWarning("Attempt to update non-existent part: {PartId}", request.Id);
            return Error.PartNotFound;
        }

        // Update properties (Code não muda)
        part.Name = request.Name;
        part.Description = request.Description;
        part.Category = request.Category;
        part.Brand = request.Brand;
        part.UnitCost = new Money(request.UnitCost, "EUR");
        part.SalePrice = new Money(request.SalePrice, "EUR");
        part.MinStockLevel = request.MinStockLevel;
        part.SupplierName = request.SupplierName;
        part.SupplierContact = request.SupplierContact;
        part.Location = request.Location;
        part.IsActive = request.IsActive;

        await _unitOfWork.PartRepository.UpdateAsync(part);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Part updated successfully: {PartId}, {PartCode}", part.Id, part.Code);

        var response = new UpdatePartResponse(
            part.Id,
            part.Code,
            part.Name,
            part.IsActive,
            part.UpdatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

