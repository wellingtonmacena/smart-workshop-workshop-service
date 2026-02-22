using SmartWorkshop.Workshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Domain.DTOs.People;

[ExcludeFromCodeCoverage]
public record PersonDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string DocumentNumber { get; init; } = string.Empty;
    public PersonType Type { get; init; }
    public ICollection<AddressDto> Addresses { get; init; } = [];
    public ICollection<PhoneDto> Phones { get; init; } = [];
    public ICollection<EmailDto> Emails { get; init; } = [];
}

[ExcludeFromCodeCoverage]
public record AddressDto(
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode);

[ExcludeFromCodeCoverage]
public record PhoneDto(string Number, PhoneType Type);

[ExcludeFromCodeCoverage]
public record EmailDto(string Address);
