using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.Entities;

public class Vehicle : Entity
{
    private Vehicle() { }

    public Vehicle(string model, string brand, int manufactureYear, string licensePlate, Guid personId) : this()
    {
        Model = model;
        Brand = brand;
        ManufactureYear = manufactureYear;
        LicensePlate = licensePlate;
        PersonId = personId;
    }

    public LicensePlate LicensePlate { get; private set; } = null!;
    public int ManufactureYear { get; private set; }
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public Guid PersonId { get; private set; }

    public Person Person { get; private set; } = null!;

    public Vehicle Update(int? manufactureYear, string licensePlate, string brand, string model)
    {
        if (manufactureYear.HasValue) ManufactureYear = manufactureYear.Value;
        if (!string.IsNullOrEmpty(licensePlate)) LicensePlate = licensePlate;
        if (!string.IsNullOrEmpty(brand)) Brand = brand;
        if (!string.IsNullOrEmpty(model)) Model = model;
        return this;
    }
}
