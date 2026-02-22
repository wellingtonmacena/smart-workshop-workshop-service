using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.States.ServiceOrder;

public abstract class ServiceOrderState
{
    public abstract ServiceOrderStatus Status { get; }
    public abstract void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status);
}
