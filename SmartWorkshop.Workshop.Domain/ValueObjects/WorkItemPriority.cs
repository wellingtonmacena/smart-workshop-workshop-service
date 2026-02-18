namespace SmartWorkshop.Workshop.Domain.ValueObjects;

/// <summary>
/// Prioridade do item de trabalho.
/// Define ordem de atendimento na fila.
/// </summary>
public enum WorkItemPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}
