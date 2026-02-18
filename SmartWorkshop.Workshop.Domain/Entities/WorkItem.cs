using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.Entities;

/// <summary>
/// Item de trabalho na fila de produção.
/// Representa a execução física de uma OS na oficina.
/// </summary>
public class WorkItem : Entity
{
    private WorkItem() { }

    public WorkItem(Guid serviceOrderId, WorkItemPriority priority = WorkItemPriority.Medium)
    {
        ServiceOrderId = serviceOrderId;
        Priority = priority;
        Status = WorkItemStatus.Pending;
        StartedAt = null;
        CompletedAt = null;
    }

    public Guid ServiceOrderId { get; private set; }
    public WorkItemStatus Status { get; private set; }
    public WorkItemPriority Priority { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string? BlockReason { get; private set; }
    public Guid? AssignedTechnicianId { get; private set; }

    public WorkItem Start(Guid technicianId)
    {
        if (Status != WorkItemStatus.Pending && Status != WorkItemStatus.Blocked)
            throw new DomainException($"Cannot start work item in status {Status}");

        Status = WorkItemStatus.InDiagnosis;
        StartedAt = DateTime.UtcNow;
        AssignedTechnicianId = technicianId;
        BlockReason = null;
        MarkAsUpdated();
        
        return this;
    }

    public WorkItem ChangePriority(WorkItemPriority newPriority)
    {
        Priority = newPriority;
        MarkAsUpdated();
        return this;
    }

    public WorkItem CompleteDiagnosis()
    {
        if (Status != WorkItemStatus.InDiagnosis)
            throw new DomainException($"Can only complete diagnosis from InDiagnosis status");

        Status = WorkItemStatus.DiagnosisCompleted;
        MarkAsUpdated();
        return this;
    }

    public WorkItem StartRepair()
    {
        if (Status != WorkItemStatus.DiagnosisCompleted)
            throw new DomainException($"Can only start repair after diagnosis is completed");

        Status = WorkItemStatus.InRepair;
        MarkAsUpdated();
        return this;
    }

    public WorkItem CompleteRepair()
    {
        if (Status != WorkItemStatus.InRepair)
            throw new DomainException($"Can only complete repair from InRepair status");

        Status = WorkItemStatus.RepairCompleted;
        MarkAsUpdated();
        return this;
    }

    public WorkItem StartQualityCheck()
    {
        if (Status != WorkItemStatus.RepairCompleted)
            throw new DomainException($"Can only start quality check after repair is completed");

        Status = WorkItemStatus.QualityCheck;
        MarkAsUpdated();
        return this;
    }

    public WorkItem Complete()
    {
        if (Status != WorkItemStatus.QualityCheck)
            throw new DomainException($"Can only complete after quality check");

        Status = WorkItemStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        MarkAsUpdated();
        return this;
    }

    public WorkItem Block(string reason)
    {
        if (Status == WorkItemStatus.Completed)
            throw new DomainException("Cannot block a completed work item");

        Status = WorkItemStatus.Blocked;
        BlockReason = reason;
        MarkAsUpdated();
        return this;
    }

    public WorkItem Unblock()
    {
        if (Status != WorkItemStatus.Blocked)
            throw new DomainException("Work item is not blocked");

        // Retorna para o estado anterior ao bloqueio
        Status = StartedAt.HasValue ? WorkItemStatus.InDiagnosis : WorkItemStatus.Pending;
        BlockReason = null;
        MarkAsUpdated();
        return this;
    }

    public TimeSpan? GetDuration()
    {
        if (StartedAt.HasValue && CompletedAt.HasValue)
            return CompletedAt.Value - StartedAt.Value;
        
        if (StartedAt.HasValue)
            return DateTime.UtcNow - StartedAt.Value;
        
        return null;
    }
}
