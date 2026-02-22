using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.States.ServiceOrder;

public sealed class InProgressState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.InProgress;

    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        if (status != ServiceOrderStatus.Completed)
        {
            throw new DomainException("Uma ordem de serviço em progresso só pode ser alterada para concluída.");
        }

        _ = serviceOrder.SetState(new CompletedState());
    }
}
