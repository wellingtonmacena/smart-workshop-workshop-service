using SmartWorkshop.Workshop.Domain.Common;

namespace SmartWorkshop.Workshop.Domain.Entities;

public sealed class Address : Entity
{
    private Address() { }

    public Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public Person Person { get; private set; } = null!;

    public void Update(Address? address)
    {
        if (address is null) return;

        if (!string.IsNullOrEmpty(address.Street)) Street = address.Street;
        if (!string.IsNullOrEmpty(address.City)) City = address.City;
        if (!string.IsNullOrEmpty(address.State)) State = address.State;
        if (!string.IsNullOrEmpty(address.ZipCode)) ZipCode = address.ZipCode;
    }
}
