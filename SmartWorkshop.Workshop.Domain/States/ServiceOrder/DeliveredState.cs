using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.States.ServiceOrder;

public sealed class DeliveredState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.Delivered;

    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        if (status != Status)
        {
            throw new DomainException("Uma ordem de serviço entregue não pode ser alterada para outro status.");
        }
    }
}
