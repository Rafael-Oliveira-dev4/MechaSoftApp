using MechaSoft.Domain.Common;
using MechaSoft.Domain.Interfaces;

namespace MechaSoft.Domain.Model;

public class Part : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Code { get; set; } // Código da peça
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string? Brand { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; } // Nível mínimo de stock
    public Money UnitCost { get; set; } // Preço de custo
    public Money SalePrice { get; set; } // Preço de venda
    public string? SupplierName { get; set; }
    public string? SupplierContact { get; set; }
    public string? Location { get; set; } // Localização no armazém
    public bool IsActive { get; set; }

    public Part()
    {
        Id = Guid.NewGuid();
        Code = Name = Description = Category = string.Empty;
        UnitCost = SalePrice = new Money(0);
        IsActive = true;
    }

    public Part(string code, string name, string description, string category,
               Money unitCost, Money salePrice, int stockQuantity, int minStockLevel)
    {
        Id = Guid.NewGuid();
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Category = category ?? throw new ArgumentNullException(nameof(category));
        UnitCost = unitCost ?? throw new ArgumentNullException(nameof(unitCost));
        SalePrice = salePrice ?? throw new ArgumentNullException(nameof(salePrice));
        StockQuantity = stockQuantity;
        MinStockLevel = minStockLevel;
        IsActive = true;
    }

    public bool IsLowStock()
    {
        return StockQuantity <= MinStockLevel;
    }

    public void UpdateStock(int quantity, StockMovementType movementType)
    {
        switch (movementType)
        {
            case StockMovementType.In:
                StockQuantity += quantity;
                break;
            case StockMovementType.Out:
                if (StockQuantity < quantity)
                    throw new InvalidOperationException("Insufficient stock");
                StockQuantity -= quantity;
                break;
        }
        UpdatedAt = DateTime.UtcNow;
    }
}
public class PartItem
{
    public Guid ServiceOrderId { get; set; }
    public Guid PartId { get; set; }
    public int Quantity { get; set; }
    public Money UnitPrice { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public Money TotalPrice { get; set; }

    // Navigation Properties
    public ServiceOrder ServiceOrder { get; set; }
    public Part Part { get; set; }

    public PartItem()
    {
        UnitPrice = TotalPrice = new Money(0);
        ServiceOrder = null!;
        Part = null!;
    }

    public PartItem(Guid serviceOrderId, Guid partId, int quantity,
                    Money unitPrice, decimal? discountPercentage = null)
    {
        TotalPrice = new Money(0);
        ServiceOrder = null!;
        Part = null!;
        ServiceOrderId = serviceOrderId;
        PartId = partId;
        Quantity = quantity;
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
        DiscountPercentage = discountPercentage;

        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        var subtotal = UnitPrice.Multiply(Quantity);

        if (DiscountPercentage.HasValue)
        {
            var discount = subtotal.Multiply(DiscountPercentage.Value / 100);
            TotalPrice = subtotal.Subtract(discount);
        }
        else
        {
            TotalPrice = subtotal;
        }
    }
}

public class PartItemRequest
{
    public Guid PartId { get; set; }
    public int Quantity { get; set; }
    public decimal? DiscountPercentage { get; set; }
}

public enum StockMovementType
{
    In,     // Entrada
    Out     // Saída
}

