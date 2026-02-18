using SmartWorkshop.Workshop.Domain.Common;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Insumo/Material utilizado nos serviços.
/// Controla estoque e preço.
/// </summary>
public class Supply : Entity
{
    private Supply() { }

    public Supply(string name, decimal price, int quantity, string? unit = null)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        Unit = unit ?? "UN";
    }

    public string Name { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public string Unit { get; private set; } = "UN"; // UN, L, KG, M, etc

    public Supply Update(string? name, decimal? price, int? quantity, string? unit)
    {
        if (!string.IsNullOrEmpty(name)) 
            Name = name;
        
        if (price.HasValue && price.Value >= 0) 
            Price = price.Value;
        
        if (quantity.HasValue && quantity.Value >= 0) 
            Quantity = quantity.Value;
        
        if (!string.IsNullOrEmpty(unit))
            Unit = unit;

        MarkAsUpdated();
        return this;
    }

    public Supply RemoveFromStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive");

        if (Quantity < quantity)
            throw new DomainException($"Insufficient stock. Available: {Quantity}, Requested: {quantity}");

        Quantity -= quantity;
        MarkAsUpdated();
        return this;
    }

    public Supply AddToStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive");

        Quantity += quantity;
        MarkAsUpdated();
        return this;
    }

    public bool IsInStock() => Quantity > 0;
    
    public bool HasSufficientStock(int requiredQuantity) => Quantity >= requiredQuantity;
}
