using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.States.ServiceOrder;

public sealed class RejectedState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.Rejected;

    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        if (status != ServiceOrderStatus.WaitingApproval)
        {
            throw new DomainException("Uma ordem de serviço rejeitada só pode ser alterada para aguardando aprovação.");
        }

        _ = serviceOrder.SetState(new WaitingApprovalState());
    }
}
