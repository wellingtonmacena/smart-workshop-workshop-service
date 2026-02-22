using SmartWorkshop.Workshop.Domain.Common;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Serviço disponível no catálogo da oficina.
/// Define tipo de serviço, preço da mão de obra e insumos necessários.
/// </summary>
public class AvailableService : Entity
{
    private AvailableService() { }

    public AvailableService(string name, decimal laborPrice, string? description = null, int? estimatedDurationMinutes = null)
    {
        Name = name;
        LaborPrice = laborPrice;
        Description = description;
        EstimatedDurationMinutes = estimatedDurationMinutes;
    }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal LaborPrice { get; private set; } // Preço da mão de obra
    public int? EstimatedDurationMinutes { get; private set; }
    
    // Navigation property
    public ICollection<ServiceOrder> ServiceOrders { get; private set; } = new List<ServiceOrder>();
    
    /// <summary>
    /// Insumos necessários para este serviço (com quantidades)
    /// </summary>
    public List<ServiceSupply> RequiredSupplies { get; private set; } = new();

    public AvailableService Update(string? name, decimal? laborPrice, string? description, int? estimatedDurationMinutes)
    {
        if (!string.IsNullOrEmpty(name)) 
            Name = name;
        
        if (laborPrice.HasValue && laborPrice.Value >= 0) 
            LaborPrice = laborPrice.Value;
        
        if (!string.IsNullOrEmpty(description))
            Description = description;

        if (estimatedDurationMinutes.HasValue && estimatedDurationMinutes.Value > 0)
            EstimatedDurationMinutes = estimatedDurationMinutes.Value;

        MarkAsUpdated();
        return this;
    }

    public AvailableService AddSupply(Guid supplyId, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Supply quantity must be positive");

        var existingSupply = RequiredSupplies.FirstOrDefault(s => s.SupplyId == supplyId);
        if (existingSupply != null)
        {
            existingSupply.UpdateQuantity(quantity);
        }
        else
        {
            RequiredSupplies.Add(new ServiceSupply(Id, supplyId, quantity));
        }

        MarkAsUpdated();
        return this;
    }

    public AvailableService RemoveSupply(Guid supplyId)
    {
        var supply = RequiredSupplies.FirstOrDefault(s => s.SupplyId == supplyId);
        if (supply != null)
        {
            RequiredSupplies.Remove(supply);
            MarkAsUpdated();
        }
        return this;
    }

    public decimal CalculateTotalPrice(Dictionary<Guid, decimal> supplyPrices)
    {
        var suppliesCost = RequiredSupplies.Sum(rs => 
        {
            if (supplyPrices.TryGetValue(rs.SupplyId, out var price))
                return price * rs.Quantity;
            return 0;
        });

        return LaborPrice + suppliesCost;
    }
}

/// <summary>
/// Relacionamento entre serviço e insumo (com quantidade)
/// </summary>
public class ServiceSupply
{
    private ServiceSupply() { }

    public ServiceSupply(Guid availableServiceId, Guid supplyId, int quantity)
    {
        AvailableServiceId = availableServiceId;
        SupplyId = supplyId;
        Quantity = quantity;
    }

    public Guid AvailableServiceId { get; private set; }
    public Guid SupplyId { get; private set; }
    public int Quantity { get; private set; }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new DomainException("Quantity must be positive");
        
        Quantity = newQuantity;
    }
}
