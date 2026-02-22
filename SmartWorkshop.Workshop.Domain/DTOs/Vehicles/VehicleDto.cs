using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Domain.DTOs.Vehicles;

[ExcludeFromCodeCoverage]
public record VehicleDto(Guid Id, string LicensePlate, int ManufactureYear, string Brand, string Model, Guid PersonId);
