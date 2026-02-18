namespace SmartWorkshop.Workshop.Domain.ValueObjects;

/// <summary>
/// Status do item de trabalho na fila de produção.
/// Representa o fluxo de execução técnica.
/// </summary>
public enum WorkItemStatus
{
    /// <summary>
    /// Item aguardando início
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Em diagnóstico técnico
    /// </summary>
    InDiagnosis = 1,

    /// <summary>
    /// Diagnóstico concluído
    /// </summary>
    DiagnosisCompleted = 2,

    /// <summary>
    /// Em reparo/manutenção
    /// </summary>
    InRepair = 3,

    /// <summary>
    /// Reparo concluído
    /// </summary>
    RepairCompleted = 4,

    /// <summary>
    /// Em controle de qualidade
    /// </summary>
    QualityCheck = 5,

    /// <summary>
    /// Trabalho completo e aprovado
    /// </summary>
    Completed = 6,

    /// <summary>
    /// Bloqueado (aguardando peça, aprovação adicional, etc)
    /// </summary>
    Blocked = 7
}
