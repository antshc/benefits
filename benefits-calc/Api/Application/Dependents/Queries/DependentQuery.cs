using Api.Application.Dependents.Payload;
using Api.Data;
using Api.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Dependents.Queries;

// why? There are no requirements to query dependents separately.
// However, let's assume such a use case exists — e.g., performing analytics, reports, or filtering (e.g., "Find all dependents over 50") across multiple employees.
public class DependentQuery : IDependentQuery
{
    private readonly BenefitsContext _context;

    public DependentQuery(BenefitsContext context)
    {
        _context = context;
    }

    public async Task<GetDependentDto> GetById(int id, CancellationToken cancellationToken)
    {
        var dep = await _context.Dependents
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken);
        if (dep is null) throw new ApplicationException("Dependent not found");

        return Map(dep);
    }

    // why? Use limit or pagination to avoid performance issues.
    public async Task<IReadOnlyCollection<GetDependentDto>> GetAll(CancellationToken cancellationToken)
    {
        var dependents = await _context.Dependents
            .AsNoTracking()
            .Select(d => new GetDependentDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Relationship = d.Relationship,
                DateOfBirth = d.DateOfBirth
            })
            .ToListAsync(cancellationToken);
        return dependents;
    }

    private static GetDependentDto Map(Dependent dep)
    {
        return new GetDependentDto
        {
            Id = dep.Id,
            FirstName = dep.FirstName,
            LastName = dep.LastName,
            Relationship = dep.Relationship,
            DateOfBirth = dep.DateOfBirth
        };
    }
}