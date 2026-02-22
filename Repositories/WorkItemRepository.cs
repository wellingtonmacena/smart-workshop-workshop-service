using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.ValueObjects;
using SmartWorkshop.Workshop.Infrastructure.Data;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SmartWorkshop.Workshop.Infrastructure.Repositories;

public sealed class WorkItemRepository(WorkshopDbContext dbContext) : Repository<WorkItem>(dbContext), IWorkItemRepository
{
    public async Task<WorkItem?> GetByServiceOrderIdAsync(Guid serviceOrderId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .FirstOrDefaultAsync(x => x.ServiceOrderId == serviceOrderId, cancellationToken);
    }

    public async Task<IEnumerable<WorkItem>> GetByStatusAsync(WorkItemStatus status, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkItem>> GetByTechnicianIdAsync(Guid technicianId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(x => x.AssignedTechnicianId == technicianId)
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.Status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkItem>> GetByPriorityAsync(WorkItemPriority priority, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(x => x.Priority == priority)
            .OrderBy(x => x.Status)
            .ThenBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
