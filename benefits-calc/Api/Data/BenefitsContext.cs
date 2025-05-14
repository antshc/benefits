using Api.Data.Configurations;
using Api.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

// why? EF Core aligns well with DDD and Onion Architecture by supporting a code-first approach,
// change tracking, LINQ-based querying, and automated schema migrations.
// More: STR21, STR22, STR23
public class BenefitsContext : DbContext
{
    public BenefitsContext(DbContextOptions<BenefitsContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Dependent> Dependents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new DependentConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}