using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class AvailableServiceConfiguration : IEntityTypeConfiguration<AvailableService>
{
    public void Configure(EntityTypeBuilder<AvailableService> builder)
    {
        builder.ToTable("available_services");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("VARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("VARCHAR(500)")
            .HasMaxLength(500);

        builder.Property(x => x.LaborPrice)
            .HasColumnName("labor_price")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();

        // Configure ServiceSupply owned entity with composite key
        builder.OwnsMany(x => x.RequiredSupplies, suppliesBuilder =>
        {
            suppliesBuilder.ToTable("service_supplies");
            
            // Composite primary key
            suppliesBuilder.HasKey(ss => new { ss.AvailableServiceId, ss.SupplyId });
            
            suppliesBuilder.Property(ss => ss.AvailableServiceId)
                .HasColumnName("available_service_id")
                .IsRequired();
            
            suppliesBuilder.Property(ss => ss.SupplyId)
                .HasColumnName("supply_id")
                .IsRequired();
            
            suppliesBuilder.Property(ss => ss.Quantity)
                .HasColumnName("quantity")
                .IsRequired();
        });
    }
}
