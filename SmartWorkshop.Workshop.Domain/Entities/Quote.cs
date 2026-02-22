using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.Entities;

public sealed class Quote : Entity
{
    private Quote() { }

    public Quote(Guid serviceOrderId)
        : this()
    {
        ServiceOrderId = serviceOrderId;
    }

    public Guid ServiceOrderId { get; private set; }
    public QuoteStatus Status { get; private set; } = QuoteStatus.Pending;
    public decimal Total { get; private set; }
    public string? Notes { get; private set; }

    // Navigation properties
    public ServiceOrder ServiceOrder { get; private set; } = null!;
    public ICollection<QuoteService> Services { get; private set; } = [];
    public ICollection<QuoteSupply> Supplies { get; private set; } = [];

    /// <summary>
    /// Adiciona um serviço ao orçamento
    /// </summary>
    public Quote AddService(Guid availableServiceId, decimal price, string serviceName)
    {
        Services.Add(new QuoteService(Id, availableServiceId, price, serviceName));
        RecalculateTotal();
        MarkAsUpdated();
        return this;
    }

    /// <summary>
    /// Adiciona um insumo/peça ao orçamento
    /// </summary>
    public Quote AddSupply(Guid supplyId, decimal price, int quantity, string supplyName)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive");

        if (price < 0)
            throw new DomainException("Price cannot be negative");

        Supplies.Add(new QuoteSupply(Id, supplyId, price, quantity, supplyName));
        RecalculateTotal();
        MarkAsUpdated();
        return this;
    }

    /// <summary>
    /// Remove um serviço do orçamento
    /// </summary>
    public Quote RemoveService(Guid quoteServiceId)
    {
        var service = Services.FirstOrDefault(s => s.Id == quoteServiceId);
        if (service != null)
        {
            Services.Remove(service);
            RecalculateTotal();
            MarkAsUpdated();
        }
        return this;
    }

    /// <summary>
    /// Remove um insumo do orçamento
    /// </summary>
    public Quote RemoveSupply(Guid quoteSupplyId)
    {
        var supply = Supplies.FirstOrDefault(s => s.Id == quoteSupplyId);
        if (supply != null)
        {
            Supplies.Remove(supply);
            RecalculateTotal();
            MarkAsUpdated();
        }
        return this;
    }

    public Quote SetNotes(string notes)
    {
        Notes = notes;
        MarkAsUpdated();
        return this;
    }

    public Quote Approve()
    {
        if (Status == QuoteStatus.Approved)
            throw new DomainException("Quote is already approved");

        if (Status == QuoteStatus.Rejected)
            throw new DomainException("Cannot approve a rejected quote");

        Status = QuoteStatus.Approved;
        MarkAsUpdated();
        return this;
    }

    public Quote Reject()
    {
        if (Status == QuoteStatus.Rejected)
            throw new DomainException("Quote is already rejected");

        if (Status == QuoteStatus.Approved)
            throw new DomainException("Cannot reject an approved quote");

        Status = QuoteStatus.Rejected;
        MarkAsUpdated();
        return this;
    }

    /// <summary>
    /// Recalcula o total do orçamento somando serviços e insumos
    /// </summary>
    public Quote RecalculateTotal()
    {
        var servicesTotal = Services.Sum(s => s.Price);
        var suppliesTotal = Supplies.Sum(s => s.GetTotal());
        Total = servicesTotal + suppliesTotal;
        return this;
    }

    /// <summary>
    /// Obtém o total somente dos serviços
    /// </summary>
    public decimal GetServicesTotal() => Services.Sum(s => s.Price);

    /// <summary>
    /// Obtém o total somente dos insumos
    /// </summary>
    public decimal GetSuppliesTotal() => Supplies.Sum(s => s.GetTotal());

    /// <summary>
    /// Verifica se o orçamento está vazio (sem serviços e sem insumos)
    /// </summary>
    public bool IsEmpty() => !Services.Any() && !Supplies.Any();
}
