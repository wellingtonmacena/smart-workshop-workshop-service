using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class QuoteSupplyConfiguration : IEntityTypeConfiguration<QuoteSupply>
{
    public void Configure(EntityTypeBuilder<QuoteSupply> builder)
    {
        builder.ToTable("quote_supplies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(x => x.QuoteId)
            .HasColumnName("quote_id")
            .IsRequired();

        builder.Property(x => x.SupplyId)
            .HasColumnName("supply_id")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(x => x.SupplyName)
            .HasColumnName("supply_name")
            .HasColumnType("VARCHAR(200)")
            .HasMaxLength(200)
            .IsRequired();

        // Relacionamento Quote -> QuoteSupplies (1:N)
        builder.HasOne(x => x.Quote)
            .WithMany(q => q.Supplies)
            .HasForeignKey(x => x.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento Supply (referência)
        builder.HasOne(x => x.Supply)
            .WithMany()
            .HasForeignKey(x => x.SupplyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
