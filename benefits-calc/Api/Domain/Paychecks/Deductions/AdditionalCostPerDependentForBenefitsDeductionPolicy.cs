using Api.Domain.Employees;

namespace Api.Domain.Paychecks.Deductions;

public class AdditionalCostPerDependentForBenefitsDeductionPolicy : IDeductionPolicy
{
    private const int MonthsPerYear = 12;
    private const decimal DependentMonthlyCost = 600m;

    public DeductionLine Calculate(Employee employee)
    {
        decimal dependentsCost = 0;

        foreach (var dependent in employee.Dependents)
        {
            dependentsCost += DependentMonthlyCost;
        }

        var annualCost = dependentsCost * MonthsPerYear;

        return new DeductionLine(nameof(AdditionalCostPerDependentForBenefitsDeductionPolicy), annualCost);
    }
}