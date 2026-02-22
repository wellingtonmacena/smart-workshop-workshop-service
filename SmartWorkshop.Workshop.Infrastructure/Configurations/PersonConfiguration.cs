using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWorkshop.Workshop.Infrastructure.Configurations;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("people");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.Fullname)
            .HasColumnName("fullname")
            .HasColumnType("VARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.PersonType)
            .HasColumnName("person_type")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.EmployeeRole)
            .HasColumnName("employee_role")
            .HasConversion<string>()
            .HasMaxLength(100);

        builder.Property(x => x.AddressId)
            .HasColumnName("address_id")
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasColumnType("VARCHAR(25)")
            .HasMaxLength(25)
            .HasConversion(new PhoneConverter())
            .IsRequired();

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(a => a.Address)
                .HasColumnName("email")
                .HasColumnType("VARCHAR(255)");

            email.HasIndex(a => a.Address).IsUnique();
        });

        builder.OwnsOne(d => d.Document, document =>
        {
            document.Property(x => x.Value)
                .HasColumnName("document")
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100)
                .IsRequired();

            document.HasIndex(x => x.Value).IsUnique();
        });

        builder.OwnsOne(d => d.Password, password =>
        {
            password.Property(x => x.Value)
                .HasColumnName("password")
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.HasOne(c => c.Address)
            .WithOne(a => a.Person)
            .HasForeignKey<Person>(c => c.AddressId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(c => c.Vehicles)
            .WithOne(x => x.Person)
            .HasForeignKey(v => v.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
