using Api.Application.Dependents.Payload;
using Api.Application.Employees.Payload;

namespace Api.Application.Dependents.Queries;

public interface IDependentQuery
{
    Task<GetDependentDto> GetById(int id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<GetDependentDto>> GetAll(CancellationToken cancellationToken);
}