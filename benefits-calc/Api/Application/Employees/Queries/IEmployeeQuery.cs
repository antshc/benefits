using Api.Application.Employees.Payload;

namespace Api.Application.Employees.Queries;

public interface IEmployeeQuery
{
    Task<IReadOnlyCollection<GetEmployeeDto>> GetAll(CancellationToken cancellationToken);

    Task<GetEmployeeDto> GetById(int id, CancellationToken cancellationToken);
}