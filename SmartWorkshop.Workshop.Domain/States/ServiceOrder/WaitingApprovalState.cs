using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.States.ServiceOrder;

public sealed class WaitingApprovalState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.WaitingApproval;

    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        _ = status switch
        {
            ServiceOrderStatus.InProgress => serviceOrder.SetState(new InProgressState()),
            ServiceOrderStatus.Rejected => serviceOrder.SetState(new RejectedState()),
            ServiceOrderStatus.Cancelled => serviceOrder.SetState(new CancelledState()),
            _ => throw new DomainException("Uma ordem de serviço esperando aprovação só pode ser alterada para em progresso, rejeitada ou cancelada.")
        };
    }
}
