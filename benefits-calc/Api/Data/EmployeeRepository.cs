using Api.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly BenefitsContext _context;

    public EmployeeRepository(BenefitsContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetById(int id, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .Include(e => e.Dependents)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}