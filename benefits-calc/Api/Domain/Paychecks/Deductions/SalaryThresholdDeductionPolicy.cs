using Api.Domain.Employees;

namespace Api.Domain.Paychecks.Deductions;

public class SalaryThresholdDeductionPolicy : IDeductionPolicy
{
    private const decimal PercentForHighSalary = 0.02m;
    private const decimal SalaryThreshold = 80000;

    public DeductionLine Calculate(Employee employee)
    {
        var annualCost = employee.Salary > SalaryThreshold ? employee.Salary * PercentForHighSalary : 0;
        return new DeductionLine(nameof(SalaryThresholdDeductionPolicy), annualCost);
    }
}