using SmartWorkshop.Workshop.Domain.DTOs.AvailableServices;
using SmartWorkshop.Workshop.Domain.DTOs.People;
using SmartWorkshop.Workshop.Domain.DTOs.Vehicles;
using SmartWorkshop.Workshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record ServiceOrderDto
{
    public Guid Id { get; init; }
    public ServiceOrderStatus Status { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid ClientId { get; init; }
    public Guid VehicleId { get; init; }
    public PersonDto? Client { get; init; }
    public VehicleDto? Vehicle { get; init; }
    public ICollection<AvailableServiceDto> AvailableServices { get; init; } = [];
    public ICollection<ServiceOrderEventDto> Events { get; init; } = [];
    public ICollection<QuoteDto> Quotes { get; init; } = [];
}
