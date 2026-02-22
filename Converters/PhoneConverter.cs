using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Infrastructure.Converters;

public class PhoneConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<Phone, string>
{
    public PhoneConverter() : base(
        v => $"{v.AreaCode} {v.Number}",
        v => new Phone(v))
    {
    }
}
