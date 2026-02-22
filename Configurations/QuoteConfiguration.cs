using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.ToTable("quotes");
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

        builder.Property(x => x.Total)
            .HasColumnName("total")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasColumnName("notes")
            .HasColumnType("VARCHAR(1000)")
            .HasMaxLength(1000);

        builder.HasOne(x => x.ServiceOrder)
            .WithMany(so => so.Quotes)
            .HasForeignKey(x => x.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Services)
            .WithOne(qs => qs.Quote)
            .HasForeignKey(qs => qs.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Supplies)
            .WithOne(qs => qs.Quote)
            .HasForeignKey(qs => qs.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
