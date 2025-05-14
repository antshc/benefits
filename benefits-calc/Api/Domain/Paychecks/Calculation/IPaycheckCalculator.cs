using Api.Domain.Employees;

namespace Api.Domain.Paychecks.Calculation;

public interface IPaycheckCalculator
{
    Paycheck Calculate(Employee employee, PaycheckPeriod period);
}