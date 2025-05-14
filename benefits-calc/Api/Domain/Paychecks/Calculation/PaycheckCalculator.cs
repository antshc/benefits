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
        var annualDeductions = CalculateAnnualDeductions(employee, period);
        short periodsPerYear = (short)period.Type;

        List<DeductionLine> perPeriodDeductions = CalculatePerPeriodDeductions(annualDeductions, periodsPerYear);

        var grossAmount = RoundToTwoDecimalPlaces(employee.Salary / periodsPerYear);
        return new Paycheck(
            grossAmount,
            period.Type,
            perPeriodDeductions,
            employee.Id
        );
    }

    private static List<DeductionLine> CalculatePerPeriodDeductions(IReadOnlyCollection<DeductionLine> annualDeductions, short periodsPerYear)
    {
        return annualDeductions.Select(d => new DeductionLine(d.Type, RoundToTwoDecimalPlaces(d.Amount / periodsPerYear))).ToList();
    }

    private IReadOnlyCollection<DeductionLine> CalculateAnnualDeductions(Employee employee, PaycheckPeriod period)
    {
        List<DeductionLine> annualDeductions = _policies.Select(p => p.Calculate(employee, period)).ToList();
        return annualDeductions;
    }
    
    private static decimal RoundToTwoDecimalPlaces(decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}