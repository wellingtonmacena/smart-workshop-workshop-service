using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Domain.DTOs.AvailableServices;

[ExcludeFromCodeCoverage]
public record AvailableServiceDto(Guid Id, string Name, decimal Price);
