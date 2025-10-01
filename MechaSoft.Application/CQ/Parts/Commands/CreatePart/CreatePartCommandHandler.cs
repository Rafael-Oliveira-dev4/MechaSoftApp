using MechaSoft.Application.Common.Responses;
using MechaSoft.Domain.Core.Uow;
using MechaSoft.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechaSoft.Application.CQ.Parts.Commands.CreatePart;

public class CreatePartCommandHandler : IRequestHandler<CreatePartCommand, Result<CreatePartResponse, Success, Error>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePartCommandHandler> _logger;

    public CreatePartCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePartCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreatePartResponse, Success, Error>> Handle(CreatePartCommand request, CancellationToken cancellationToken)
    {
        var unitCost = new Money(request.UnitCost, "EUR");
        var salePrice = new Money(request.SalePrice, "EUR");

        var part = new Part(
            request.Code,
            request.Name,
            request.Description,
            request.Category,
            unitCost,
            salePrice,
            request.StockQuantity,
            request.MinStockLevel
        );

        part.Brand = request.Brand;
        part.SupplierName = request.SupplierName;
        part.SupplierContact = request.SupplierContact;
        part.Location = request.Location;

        var savedPart = await _unitOfWork.PartRepository.SaveAsync(part);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Part created successfully: {PartId}, {PartCode}, {PartName}", 
            savedPart.Id, savedPart.Code, savedPart.Name);

        var response = new CreatePartResponse(
            savedPart.Id,
            savedPart.Code,
            savedPart.Name,
            savedPart.Category,
            savedPart.UnitCost.Amount,
            savedPart.SalePrice.Amount,
            savedPart.StockQuantity,
            savedPart.CreatedAt ?? DateTime.UtcNow
        );

        return response;
    }
}

