using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record QuoteDto
{
    public Guid Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal TotalPrice { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ApprovedAt { get; init; }
    public bool IsApproved { get; init; }
}
