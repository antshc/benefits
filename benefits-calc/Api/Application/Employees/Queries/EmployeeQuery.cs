using System.Linq.Expressions;
using Api.Application.Dependents.Payload;
using Api.Application.Employees.Payload;
using Api.Data;
using Api.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Employees.Queries;

// why? Use of a query service to encapsulate the logic for retrieving data from the database. Can be replaced with any ORM or data access technology.
// Using DbContext directly for projections allows you to bypass aggregates and return DTOs tailored for specific read scenarios.
public class EmployeeQuery : IEmployeeQuery
{
    private readonly BenefitsContext _context;

    public EmployeeQuery(BenefitsContext context)
    {
        _context = context;
    }

    // why? we need to use limit and offset to paginate the results
    // it helps to reduce the amount of data sent over the network and reduce performance issues
    public async Task<IReadOnlyCollection<GetEmployeeDto>> GetAll(CancellationToken cancellationToken)
    {
        var employees = await _context
            .Employees
            .AsNoTracking()
            .Select(GetEmployeeProjection)
            .ToListAsync(cancellationToken);
        return employees;
    }

    // why? The generated SQL includes a subquery in the JOIN clause, which may be suboptimal for performance.
    // Consider using raw SQL, rewriting the LINQ expression, or applying AsSplitQuery().
    // However, review the actual execution plan and SQL performance before making changes.
    public async Task<GetEmployeeDto> GetById(int id, CancellationToken cancellationToken)
    {
        var emp = await _context.Employees
            .Include(e => e.Dependents)
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(GetEmployeeProjection)
            .FirstOrDefaultAsync(cancellationToken);
        if (emp is null) throw new ApplicationException("Employee not found");
        return emp;
    }

    private static readonly Expression<Func<Employee, GetEmployeeDto>> GetEmployeeProjection = e => new GetEmployeeDto
    {
        Id = e.Id,
        FirstName = e.FirstName,
        LastName = e.LastName,
        Salary = e.Salary,
        DateOfBirth = e.DateOfBirth,
        Dependents = e.Dependents.Select(d => new GetDependentDto
        {
            Id = d.Id,
            FirstName = d.FirstName,
            LastName = d.LastName,
            Relationship = d.Relationship,
            DateOfBirth = d.DateOfBirth
        }).ToList()
    };
}