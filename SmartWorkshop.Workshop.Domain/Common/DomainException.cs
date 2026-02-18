namespace SmartWorkshop.Workshop.Domain.Common;

/// <summary>
/// Exceção base para erros de regras de domínio.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
