using Api.Domain.Employees;
using Api.Domain.Paychecks.Deductions;

namespace Api.Domain.Paychecks.Calculation;

// why? Encapsulates the business logic for calculating a paycheck based on employee data and deduction policy rules.
// Deduction policies can be filtered before use and may be managed independently in the future, each with its own ID.
// This logic does not belong in the Paycheck aggregate (which is the output) and should not overload the Employee aggregate with calculation responsibilities.
public class PaycheckCalculator : IPaycheckCalculator
{
    private readonly IEnumerable<IDeductionPolicy> _policies;

    public PaycheckCalculator(IEnumerable<IDeductionPolicy> policies)
    {
        _policies = policies;
    }

    public Paycheck Calculate(Employee employee, PaycheckPeriod period)
    {
        var annualDeductions = CalculateAnnualDeductions(employee);

        return new Paycheck(
            employee.Salary,
            period,
            annualDeductions,
            employee.Id
        );
    }

    private IReadOnlyCollection<DeductionLine> CalculateAnnualDeductions(Employee employee)
    {
        List<DeductionLine> annualDeductions = _policies.Select(p => p.Calculate(employee)).ToList();
        return annualDeductions;
    }
}