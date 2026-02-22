using SmartWorkshop.Workshop.Domain.Common;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Log detalhado de diagnóstico técnico.
/// Armazenado em MongoDB devido à estrutura flexível (fotos, medições variáveis).
/// </summary>
public class DiagnosticLog : Entity
{
    private DiagnosticLog() { }

    public DiagnosticLog(Guid workItemId, Guid technicianId, string description)
    {
        WorkItemId = workItemId;
        TechnicianId = technicianId;
        Description = description;
        Timestamp = DateTime.UtcNow;
        PhotoUrls = new List<string>();
        Measurements = new Dictionary<string, string>();
        Findings = new List<string>();
    }

    public Guid WorkItemId { get; private set; }
    public Guid TechnicianId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; }
    
    /// <summary>
    /// URLs das fotos do diagnóstico (S3, blob storage, etc)
    /// </summary>
    public List<string> PhotoUrls { get; private set; } = new();
    
    /// <summary>
    /// Medições realizadas (ex: "pressao_pneu_dianteiro_esquerdo": "32 PSI")
    /// Schema flexível permite diferentes tipos de medições por veículo
    /// </summary>
    public Dictionary<string, string> Measurements { get; private set; } = new();
    
    /// <summary>
    /// Lista de problemas identificados
    /// </summary>
    public List<string> Findings { get; private set; } = new();

    public DiagnosticLog AddPhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new DomainException("Photo URL cannot be empty");

        PhotoUrls.Add(photoUrl);
        MarkAsUpdated();
        return this;
    }

    public DiagnosticLog AddMeasurement(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new DomainException("Measurement key cannot be empty");

        Measurements[key] = value;
        MarkAsUpdated();
        return this;
    }

    public DiagnosticLog AddFinding(string finding)
    {
        if (string.IsNullOrWhiteSpace(finding))
            throw new DomainException("Finding cannot be empty");

        Findings.Add(finding);
        MarkAsUpdated();
        return this;
    }

    public DiagnosticLog Update(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description))
            Description = description;

        MarkAsUpdated();
        return this;
    }
}
