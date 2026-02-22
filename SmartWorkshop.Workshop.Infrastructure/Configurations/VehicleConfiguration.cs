using SmartWorkshop.Workshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Property(x => x.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(x => x.ManufactureYear)
            .HasColumnName("manufacture_year")
            .IsRequired();

        builder.Property(x => x.Brand)
            .HasColumnName("brand")
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Model)
            .HasColumnName("model")
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(v => v.LicensePlate, licensePlate =>
        {
            licensePlate.Property(lp => lp.Value)
                .HasColumnName("license_plate")
                .HasColumnType("VARCHAR(10)")
                .HasMaxLength(10)
                .IsRequired();

            licensePlate.HasIndex(lp => lp.Value).IsUnique();
        });

        builder.HasOne(v => v.Person)
            .WithMany(p => p.Vehicles)
            .HasForeignKey(v => v.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
