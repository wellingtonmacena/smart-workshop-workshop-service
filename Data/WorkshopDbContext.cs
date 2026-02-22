using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Infrastructure.Data;

[ExcludeFromCodeCoverage]
public class WorkshopDbContext(DbContextOptions<WorkshopDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Person> People { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<AvailableService> AvailableServices { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<QuoteService> QuoteServices { get; set; }
    public DbSet<QuoteSupply> QuoteSupplies { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<ServiceOrderEvent> EventLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new AvailableServiceConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceOrderConfiguration());
        modelBuilder.ApplyConfiguration(new QuoteConfiguration());
        modelBuilder.ApplyConfiguration(new QuoteServiceConfiguration());
        modelBuilder.ApplyConfiguration(new QuoteSupplyConfiguration());
        modelBuilder.ApplyConfiguration(new WorkItemConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceOrderEventConfiguration());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntries = ChangeTracker.Entries().Where(e => e is { State: EntityState.Modified, Entity: Entity });
        foreach (var entry in modifiedEntries) ((Entity)entry.Entity).MarkAsUpdated();
        return base.SaveChangesAsync(cancellationToken);
    }
}
