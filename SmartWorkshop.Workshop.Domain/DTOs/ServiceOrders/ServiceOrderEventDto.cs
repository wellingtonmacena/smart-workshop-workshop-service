using SmartWorkshop.Workshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record ServiceOrderEventDto
{
    public Guid Id { get; init; }
    public ServiceOrderStatus Status { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}
