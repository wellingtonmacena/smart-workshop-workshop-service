using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    private const string AvailableServiceId = "available_service_id";
    private const string ServiceOrderId = "service_order_id";

    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("service_orders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.ClientId).HasColumnName("client_id").IsRequired();
        builder.Property(x => x.VehicleId).HasColumnName("vehicle_id").IsRequired();
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("VARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasColumnType("VARCHAR(250)")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(x => x.ApprovedQuoteId)
            .HasColumnName("approved_quote_id")
            .IsRequired(false);

        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Vehicle)
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Events)
            .WithOne(x => x.ServiceOrder)
            .HasForeignKey(x => x.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Quotes)
            .WithOne(x => x.ServiceOrder)
            .HasForeignKey(x => x.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AvailableServices)
            .WithMany(x => x.ServiceOrders)
            .UsingEntity<Dictionary<string, object>>(
                "available_services_service_orders",
                j => j
                    .HasOne<AvailableService>()
                    .WithMany()
                    .HasForeignKey(AvailableServiceId)
                    .HasConstraintName("fk_available_service_service_order")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<ServiceOrder>()
                    .WithMany()
                    .HasForeignKey(ServiceOrderId)
                    .HasConstraintName("fk_service_order_available_service")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey(AvailableServiceId, ServiceOrderId);
                    j.Property<Guid>(AvailableServiceId).HasColumnName(AvailableServiceId);
                    j.Property<Guid>(ServiceOrderId).HasColumnName(ServiceOrderId);
                    j.ToTable("available_services_service_orders");
                }
            );
    }
}
