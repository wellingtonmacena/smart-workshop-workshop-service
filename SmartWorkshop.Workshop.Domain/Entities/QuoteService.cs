namespace SmartWorkshop.Workshop.Domain.Entities;

public sealed class QuoteService
{
    private QuoteService() { }

    public QuoteService(Guid quoteId, Guid serviceId, decimal price, string serviceName)
    {
        QuoteId = quoteId;
        ServiceId = serviceId;
        Price = price;
        ServiceName = serviceName;
    }

    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid ServiceId { get; private set; }
    public decimal Price { get; private set; }
    public string ServiceName { get; private set; } = string.Empty;
    public Quote Quote { get; private set; } = null!;
    public AvailableService Service { get; private set; } = null!;
}
