using Api.Domain.Employees;

namespace Api.Domain.Paychecks.Deductions;

public class BaseCostForBenefitsDeductionPolicy : IDeductionPolicy
{
    private const int MonthsPerYear = 12;
    private const decimal BaseCostPerMonth = 1000m;

    public DeductionLine Calculate(Employee employee, PaycheckPeriod paycheckPeriod)
    {
        var annualCost = BaseCostPerMonth * MonthsPerYear;
        return new DeductionLine(nameof(BaseCostForBenefitsDeductionPolicy), annualCost);
    }
}