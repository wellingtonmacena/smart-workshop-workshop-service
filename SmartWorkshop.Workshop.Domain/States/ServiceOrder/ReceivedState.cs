using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.States.ServiceOrder;

public sealed class ReceivedState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.Received;

    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        if (status != ServiceOrderStatus.UnderDiagnosis)
        {
            throw new DomainException("Uma ordem de serviço recebida só pode ser alterada para sob diagnóstico.");
        }

        _ = serviceOrder.SetState(new UnderDiagnosisState());
    }
}
