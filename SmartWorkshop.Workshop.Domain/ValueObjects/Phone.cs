namespace SmartWorkshop.Workshop.Domain.ValueObjects;

public record Phone
{
    public Phone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return;

        string[] parts = phone.Split(' ', 2);
        AreaCode = parts[0];
        Number = parts.Length > 1 ? parts[1] : string.Empty;
    }

    public Phone(string areaCode, string number)
    {
        AreaCode = areaCode;
        Number = number;
    }

    private Phone() { }

    public string AreaCode { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;

    public static implicit operator Phone(string phone) => new Phone(phone);
    public static implicit operator string(Phone phone) => $"{phone.AreaCode} {phone.Number}";
}
