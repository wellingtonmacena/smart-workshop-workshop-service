using SmartWorkshop.Workshop.Domain.Common;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Representa um insumo/peça incluído em um orçamento.
/// Armazena preço e quantidade no momento do orçamento.
/// </summary>
public sealed class QuoteSupply
{
    private QuoteSupply() { }

    public QuoteSupply(Guid quoteId, Guid supplyId, decimal price, int quantity, string supplyName)
    {
        QuoteId = quoteId;
        SupplyId = supplyId;
        Price = price;
        Quantity = quantity;
        SupplyName = supplyName;
    }

    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid SupplyId { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public string SupplyName { get; private set; } = string.Empty;
    
    // Navigation properties
    public Quote Quote { get; private set; } = null!;
    public Supply Supply { get; private set; } = null!;

    /// <summary>
    /// Calcula o total do item (preço unitário × quantidade)
    /// </summary>
    public decimal GetTotal() => Price * Quantity;

    /// <summary>
    /// Atualiza a quantidade do insumo no orçamento
    /// </summary>
    public QuoteSupply UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new DomainException("Quantity must be positive");

        Quantity = newQuantity;
        return this;
    }

    /// <summary>
    /// Atualiza o preço do insumo no orçamento
    /// </summary>
    public QuoteSupply UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new DomainException("Price cannot be negative");

        Price = newPrice;
        return this;
    }
}
