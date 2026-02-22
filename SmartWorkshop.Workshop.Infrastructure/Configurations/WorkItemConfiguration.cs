using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable("work_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Property(x => x.ServiceOrderId)
            .HasColumnName("service_order_id")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasColumnName("priority")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.StartedAt)
            .HasColumnName("started_at");

        builder.Property(x => x.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(x => x.BlockReason)
            .HasColumnName("block_reason")
            .HasColumnType("VARCHAR(1000)")
            .HasMaxLength(1000);

        builder.Property(x => x.AssignedTechnicianId)
            .HasColumnName("assigned_technician_id");

        // Relacionamento com ServiceOrder
        builder.HasOne<ServiceOrder>()
            .WithMany()
            .HasForeignKey(x => x.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento com Technician (Person)
        builder.HasOne<Person>()
            .WithMany()
            .HasForeignKey(x => x.AssignedTechnicianId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para performance
        builder.HasIndex(x => x.ServiceOrderId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.AssignedTechnicianId);
        builder.HasIndex(x => x.Priority);
    }
}
