using SmartWorkshop.Workshop.Domain.Common;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Log detalhado de reparo/manutenção executada.
/// Armazenado em MongoDB devido à estrutura flexível (fotos, peças variáveis).
/// </summary>
public class RepairLog : Entity
{
    private RepairLog() { }

    public RepairLog(Guid workItemId, Guid technicianId, string description)
    {
        WorkItemId = workItemId;
        TechnicianId = technicianId;
        Description = description;
        Timestamp = DateTime.UtcNow;
        PhotoUrls = new List<string>();
        PartsUsed = new List<PartUsage>();
        Measurements = new Dictionary<string, string>();
    }

    public Guid WorkItemId { get; private set; }
    public Guid TechnicianId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// URLs das fotos do reparo (antes/depois, peças substituídas, etc)
    /// </summary>
    public List<string> PhotoUrls { get; private set; } = new();

    /// <summary>
    /// Peças/insumos utilizados no reparo
    /// </summary>
    public List<PartUsage> PartsUsed { get; private set; } = new();

    /// <summary>
    /// Medições pós-reparo (torque, pressão, alinhamento, etc)
    /// Schema flexível permite diferentes tipos de medições
    /// </summary>
    public Dictionary<string, string> Measurements { get; private set; } = new();

    public RepairLog AddPhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new DomainException("Photo URL cannot be empty");

        PhotoUrls.Add(photoUrl);
        MarkAsUpdated();
        return this;
    }

    public RepairLog AddPartUsed(Guid supplyId, string partName, int quantity, string? notes = null)
    {
        if (quantity <= 0)
            throw new DomainException("Part quantity must be positive");

        PartsUsed.Add(new PartUsage
        {
            SupplyId = supplyId,
            PartName = partName,
            Quantity = quantity,
            Notes = notes
        });

        MarkAsUpdated();
        return this;
    }

    public RepairLog AddMeasurement(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new DomainException("Measurement key cannot be empty");

        Measurements[key] = value;
        MarkAsUpdated();
        return this;
    }

    public RepairLog Update(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description))
            Description = description;

        MarkAsUpdated();
        return this;
    }
}

/// <summary>
/// Peça/insumo utilizado em um reparo
/// </summary>
public class PartUsage
{
    public Guid SupplyId { get; set; }
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
