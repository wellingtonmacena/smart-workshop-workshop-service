using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Evento de mudança de status de uma Ordem de Serviço.
/// Mantém histórico completo de transições para auditoria.
/// </summary>
public class ServiceOrderEvent : Entity
{
    private ServiceOrderEvent() { }

    public ServiceOrderEvent(Guid serviceOrderId, ServiceOrderStatus fromStatus, ServiceOrderStatus toStatus, string? reason = null)
    {
        ServiceOrderId = serviceOrderId;
        FromStatus = fromStatus;
        ToStatus = toStatus;
        Reason = reason;
        OccurredAt = DateTime.UtcNow;
    }

    public Guid ServiceOrderId { get; private set; }
    public ServiceOrderStatus FromStatus { get; private set; }
    public ServiceOrderStatus ToStatus { get; private set; }
    public string? Reason { get; private set; }
    public DateTime OccurredAt { get; private set; }

    // Navigation property
    public ServiceOrder? ServiceOrder { get; private set; }
}
