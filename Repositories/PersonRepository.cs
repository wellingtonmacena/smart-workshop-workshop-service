using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Data;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SmartWorkshop.Workshop.Infrastructure.Repositories;

public sealed class PersonRepository(WorkshopDbContext dbContext) : Repository<Person>(dbContext), IPersonRepository
{
    public async Task<Person?> GetByDocumentAsync(string document, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(p => p.Address)
            .Include(p => p.Vehicles)
            .FirstOrDefaultAsync(p => p.Document.Value == document, cancellationToken);
    }
}
