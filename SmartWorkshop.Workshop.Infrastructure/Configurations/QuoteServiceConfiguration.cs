using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class QuoteServiceConfiguration : IEntityTypeConfiguration<QuoteService>
{
    public void Configure(EntityTypeBuilder<QuoteService> builder)
    {
        builder.ToTable("quote_services");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(x => x.QuoteId)
            .HasColumnName("quote_id")
            .IsRequired();

        builder.Property(x => x.ServiceId)
            .HasColumnName("service_id")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();

        builder.Property(x => x.ServiceName)
            .HasColumnName("service_name")
            .HasColumnType("VARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(x => x.Quote)
            .WithMany(q => q.Services)
            .HasForeignKey(x => x.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
