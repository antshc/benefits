using Api.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class DependentConfiguration : IEntityTypeConfiguration<Dependent>
{
    public void Configure(EntityTypeBuilder<Dependent> builder)
    {
        // Map to table "Dependents"
        builder.ToTable("Dependents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.FirstName)
            .HasMaxLength(100);

        builder.Property(d => d.LastName)
            .HasMaxLength(100);

        // why? Consider use the DateOnly type to represent a DateOfBirth
        builder.Property(d => d.DateOfBirth)
            .IsRequired();

        builder.Property(d => d.Relationship)
            .IsRequired();

        builder
            .HasOne(e => e.Employee)
            .WithMany(e => e.Dependents)
            .HasForeignKey(d => d.EmployeeId)
            .IsRequired();
    }
}