using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Data;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;

namespace SmartWorkshop.Workshop.Infrastructure.Repositories;

public sealed class SupplyRepository(WorkshopDbContext dbContext) : Repository<Supply>(dbContext), ISupplyRepository
{
}
