using System.Security.Cryptography;
using System.Text;

namespace SmartWorkshop.Workshop.Domain.ValueObjects;

public record Password
{
    private Password() { }

    private Password(string value)
    {
        Value = HashPassword(value);
    }

    public string Value { get; private set; } = string.Empty;

    public bool Verify(string password)
    {
        return Value == HashPassword(password);
    }

    private static string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return string.Empty;

        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public static implicit operator Password(string value) => new Password(value);
    public static implicit operator string(Password password) => password.Value;
}
