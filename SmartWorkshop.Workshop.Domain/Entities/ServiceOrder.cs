using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.States.ServiceOrder;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Ordem de Serviço - Agregado principal do contexto de oficina.
/// Gerencia o ciclo completo de atendimento desde recebimento até entrega.
/// </summary>
public class ServiceOrder : Entity
{
    private ServiceOrderState _state = new ReceivedState();

    private ServiceOrder() { }

    public ServiceOrder(string title, string description, Guid vehicleId, Guid clientId)
    {
        Title = title;
        Description = description;
        VehicleId = vehicleId;
        ClientId = clientId;
        Status = ServiceOrderStatus.Received;
        _state = new ReceivedState();
    }

    public ServiceOrderStatus Status { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    // Navigation properties
    public Person? Client { get; private set; }
    public Vehicle? Vehicle { get; private set; }
    public List<Quote> Quotes { get; private set; } = new();
    public List<AvailableService> AvailableServices { get; private set; } = new();

    /// <summary>
    /// Serviços incluídos nesta OS
    /// </summary>
    public List<Guid> AvailableServiceIds { get; private set; } = new();

    /// <summary>
    /// Eventos de mudança de status (auditoria)
    /// </summary>
    public List<ServiceOrderEvent> Events { get; private set; } = new();

    /// <summary>
    /// ID do orçamento aprovado (se houver)
    /// </summary>
    public Guid? ApprovedQuoteId { get; private set; }

    public ServiceOrder AddService(Guid serviceId)
    {
        if (!CanBeUpdated())
            throw new DomainException($"Service Order with status {Status} cannot be updated.");

        if (!AvailableServiceIds.Contains(serviceId))
            AvailableServiceIds.Add(serviceId);

        return this;
    }

    public ServiceOrder Update(string? title, string? description, List<Guid>? serviceIds)
    {
        if (!CanBeUpdated())
            throw new DomainException($"Service Order with status {Status} cannot be updated.");

        if (!string.IsNullOrEmpty(title))
            Title = title;

        if (!string.IsNullOrEmpty(description))
            Description = description;

        if (serviceIds != null)
            AvailableServiceIds = serviceIds;

        MarkAsUpdated();
        return this;
    }

    public ServiceOrder SetState(ServiceOrderState state)
    {
        _state = state;
        Status = state.Status;
        return this;
    }

    public ServiceOrder ChangeStatus(ServiceOrderStatus newStatus, string? reason = null)
    {
        // Delega ao State pattern para validar e transicionar
        _state.ChangeStatus(this, newStatus);

        // Registra o evento de mudança de status
        Events.Add(new ServiceOrderEvent(Id, Status, newStatus, reason));
        MarkAsUpdated();

        return this;
    }

    public ServiceOrder SyncState()
    {
        _state = Status switch
        {
            ServiceOrderStatus.Received => new ReceivedState(),
            ServiceOrderStatus.UnderDiagnosis => new UnderDiagnosisState(),
            ServiceOrderStatus.WaitingApproval => new WaitingApprovalState(),
            ServiceOrderStatus.InProgress => new InProgressState(),
            ServiceOrderStatus.Completed => new CompletedState(),
            ServiceOrderStatus.Delivered => new DeliveredState(),
            ServiceOrderStatus.Cancelled => new CancelledState(),
            ServiceOrderStatus.Rejected => new RejectedState(),
            _ => throw new InvalidOperationException($"Unknown status: {Status}")
        };
        return this;
    }

    public ServiceOrder ApproveQuote(Guid quoteId)
    {
        if (Status != ServiceOrderStatus.WaitingApproval)
            throw new DomainException($"Cannot approve quote. Service Order must be in WaitingApproval status.");

        ApprovedQuoteId = quoteId;
        ChangeStatus(ServiceOrderStatus.InProgress, "Quote approved");

        return this;
    }

    public ServiceOrder RejectQuote(string reason)
    {
        if (Status != ServiceOrderStatus.WaitingApproval)
            throw new DomainException($"Cannot reject quote. Service Order must be in WaitingApproval status.");

        ChangeStatus(ServiceOrderStatus.Rejected, reason);

        return this;
    }

    private bool CanBeUpdated()
    {
        return Status is ServiceOrderStatus.Received
            or ServiceOrderStatus.UnderDiagnosis
            or ServiceOrderStatus.WaitingApproval;
    }

    private void ValidateStatusTransition(ServiceOrderStatus newStatus)
    {
        var validTransitions = Status switch
        {
            ServiceOrderStatus.Received => new[] { ServiceOrderStatus.UnderDiagnosis, ServiceOrderStatus.Cancelled },
            ServiceOrderStatus.UnderDiagnosis => new[] { ServiceOrderStatus.WaitingApproval, ServiceOrderStatus.Cancelled },
            ServiceOrderStatus.WaitingApproval => new[] { ServiceOrderStatus.InProgress, ServiceOrderStatus.Rejected, ServiceOrderStatus.Cancelled },
            ServiceOrderStatus.InProgress => new[] { ServiceOrderStatus.Completed, ServiceOrderStatus.Cancelled },
            ServiceOrderStatus.Completed => new[] { ServiceOrderStatus.Delivered },
            ServiceOrderStatus.Delivered => Array.Empty<ServiceOrderStatus>(),
            ServiceOrderStatus.Cancelled => Array.Empty<ServiceOrderStatus>(),
            ServiceOrderStatus.Rejected => new[] { ServiceOrderStatus.WaitingApproval }, // Pode refazer orçamento
            _ => Array.Empty<ServiceOrderStatus>()
        };

        if (!validTransitions.Contains(newStatus))
        {
            throw new DomainException($"Invalid status transition from {Status} to {newStatus}");
        }
    }
}
