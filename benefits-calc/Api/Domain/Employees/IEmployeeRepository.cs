namespace Api.Domain.Employees;

public interface IEmployeeRepository
{
    Task<Employee?> GetById(int id, CancellationToken cancellationToken);
}