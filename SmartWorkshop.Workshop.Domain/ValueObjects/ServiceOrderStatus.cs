namespace SmartWorkshop.Workshop.Domain.ValueObjects;

/// <summary>
/// Status possíveis de uma Ordem de Serviço.
/// Representa o ciclo de vida completo desde recebimento até entrega.
/// </summary>
public enum ServiceOrderStatus
{
    /// <summary>
    /// OS recebida, aguardando início do diagnóstico
    /// </summary>
    Received = 0,

    /// <summary>
    /// Veículo em diagnóstico técnico
    /// </summary>
    UnderDiagnosis = 1,

    /// <summary>
    /// Diagnóstico completo, aguardando aprovação do orçamento pelo cliente
    /// </summary>
    WaitingApproval = 2,

    /// <summary>
    /// Orçamento aprovado, serviço em execução
    /// </summary>
    InProgress = 3,

    /// <summary>
    /// Serviço concluído, aguardando retirada
    /// </summary>
    Completed = 4,

    /// <summary>
    /// Veículo entregue ao cliente
    /// </summary>
    Delivered = 5,

    /// <summary>
    /// OS cancelada antes da conclusão
    /// </summary>
    Cancelled = 6,

    /// <summary>
    /// Orçamento rejeitado pelo cliente
    /// </summary>
    Rejected = 7
}
