using Api.Domain.Employees;

namespace Api.Domain.Paychecks.Deductions;

public interface IDeductionPolicy
{
    DeductionLine Calculate(Employee employee);
}