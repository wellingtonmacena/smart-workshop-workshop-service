using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Data;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SmartWorkshop.Workshop.Infrastructure.Repositories;

public sealed class ServiceOrderRepository(WorkshopDbContext dbContext) : Repository<ServiceOrder>(dbContext), IServiceOrderRepository
{
    public override async Task<ServiceOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await base.GetByIdAsync(id, cancellationToken);
        return result?.SyncState();
    }

    public async Task<ServiceOrder?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(x => x.AvailableServices)
            .Include(x => x.Client)
            .Include(x => x.Vehicle)
            .Include(x => x.Quotes).ThenInclude(q => q.Services)
            .Include(x => x.Events)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
