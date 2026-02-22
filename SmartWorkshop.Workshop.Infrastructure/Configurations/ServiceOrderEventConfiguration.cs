using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class ServiceOrderEventConfiguration : IEntityTypeConfiguration<ServiceOrderEvent>
{
    public void Configure(EntityTypeBuilder<ServiceOrderEvent> builder)
    {
        builder.ToTable("service_order_events");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Property(x => x.ServiceOrderId)
            .HasColumnName("service_order_id")
            .IsRequired();

        builder.Property(x => x.FromStatus)
            .HasColumnName("from_status")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(x => x.ToStatus)
            .HasColumnName("to_status")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasColumnName("reason")
            .HasColumnType("VARCHAR(500)")
            .HasMaxLength(500);

        builder.HasOne(x => x.ServiceOrder)
            .WithMany(so => so.Events)
            .HasForeignKey(x => x.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
