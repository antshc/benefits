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
        // why? Use of a projection to select only the necessary fields from the database.
        var employees = await _context
            .Employees
            .AsNoTracking()
            .Select(e => new GetEmployeeDto
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
            }).ToListAsync(cancellationToken);
        return employees;
    }

    public async Task<GetEmployeeDto> GetById(int id, CancellationToken cancellationToken)
    {
        Employee? emp = await _context.Employees
            .Include(e => e.Dependents)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (emp is null) throw new ApplicationException("Employee not found");
        return Map(emp);
    }

    // Why? Use auto mapper which simplifies object-to-object mapping and reduces boilerplate code.
    private static GetEmployeeDto Map(Employee emp)
    {
        return new GetEmployeeDto
        {
            Id = emp.Id,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            Salary = emp.Salary,
            DateOfBirth = emp.DateOfBirth,
            Dependents = emp.Dependents.Select(d => new GetDependentDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Relationship = d.Relationship,
                DateOfBirth = d.DateOfBirth
            }).ToList()
        };
    }
}