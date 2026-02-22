using System.Text.RegularExpressions;

namespace SmartWorkshop.Workshop.Domain.ValueObjects;

public record Email
{
    private Email() { }

    private Email(string address)
    {
        Address = address;
    }

    public string Address { get; private set; } = string.Empty;

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Address)) return false;

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.NonBacktracking);
        return emailRegex.IsMatch(Address);
    }

    public static implicit operator Email(string address) => new Email(address);
    public static implicit operator string(Email email) => email.Address;
}
