using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Property(x => x.Street)
            .HasColumnName("street")
            .HasColumnType("VARCHAR(255)")
            .HasMaxLength(255);

        builder.Property(x => x.City)
            .HasColumnName("city")
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasColumnName("state")
            .HasColumnType("VARCHAR(2)")
            .HasMaxLength(2);

        builder.Property(x => x.ZipCode)
            .HasColumnName("zip_code")
            .HasColumnType("VARCHAR(10)")
            .HasMaxLength(10);
    }
}
