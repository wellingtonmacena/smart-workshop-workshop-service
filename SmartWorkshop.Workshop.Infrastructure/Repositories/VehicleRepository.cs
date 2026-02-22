using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Data;
using SmartWorkshop.Workshop.Application.Adapters.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SmartWorkshop.Workshop.Infrastructure.Repositories;

public sealed class VehicleRepository(WorkshopDbContext dbContext) : Repository<Vehicle>(dbContext), IVehicleRepository
{
    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(v => v.Person)
            .FirstOrDefaultAsync(v => v.LicensePlate.Value == licensePlate, cancellationToken);
    }
}
