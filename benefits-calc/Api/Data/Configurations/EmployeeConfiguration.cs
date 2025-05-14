using Api.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        // why? Consider indexes are created for commonly queried fields
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Salary)
            .IsRequired()
            .HasPrecision(18, 2);

        // why? Consider use the DateOnly type to represent a DateOfBirth
        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        // why? Consider to use owned-entities, because Dependent not exist outside Employee https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities
        builder.HasMany(e => e.Dependents)
            .WithOne(d => d.Employee)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}