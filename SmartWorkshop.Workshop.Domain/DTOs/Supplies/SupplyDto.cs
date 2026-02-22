using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Domain.DTOs.Supplies;

[ExcludeFromCodeCoverage]
public record SupplyDto(Guid Id, string Name, int Quantity, decimal Price);
