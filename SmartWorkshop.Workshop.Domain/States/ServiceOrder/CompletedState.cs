using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.States.ServiceOrder;

public sealed class CompletedState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.Completed;

    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        if (status != ServiceOrderStatus.Delivered)
        {
            throw new DomainException("Uma ordem de serviço concluída só pode ser alterada para entregue.");
        }

        _ = serviceOrder.SetState(new DeliveredState());
    }
}
