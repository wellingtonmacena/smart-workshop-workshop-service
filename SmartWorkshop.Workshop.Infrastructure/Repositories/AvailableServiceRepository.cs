using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Data;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;

namespace SmartWorkshop.Workshop.Infrastructure.Repositories;

public sealed class AvailableServiceRepository(WorkshopDbContext dbContext) : Repository<AvailableService>(dbContext), IAvailableServiceRepository
{
}
