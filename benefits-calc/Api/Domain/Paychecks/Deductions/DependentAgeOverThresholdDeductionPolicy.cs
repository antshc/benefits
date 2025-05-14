using Api.Domain.Employees;
using Api.SharedKernel;

namespace Api.Domain.Paychecks.Deductions;

public class DependentAgeOverThresholdDeductionPolicy : IDeductionPolicy
{
    private const int MonthsPerYear = 12;
    private readonly IDateTimeProvider _dateTimeProvider;
    private const decimal AdditionalCostForOlderDependents = 200m;
    private const int AgeOverThreshold = 50;

    public DependentAgeOverThresholdDeductionPolicy(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public DeductionLine Calculate(Employee employee, PaycheckPeriod paycheckPeriod)
    {
        decimal annualCost = 0;

        // why? this logic probably related to Employee's grandparents, but for now the only place where it potentially fit is there
        if (employee.SpouseOrDomesticPartner is not null && IsAgeOverThreshold(employee.SpouseOrDomesticPartner))
        {
            annualCost += AdditionalCostForOlderDependents * MonthsPerYear;
        }

        return new DeductionLine(nameof(DependentAgeOverThresholdDeductionPolicy), annualCost);
    }

    private bool IsAgeOverThreshold(Dependent dependent)
    {
        return dependent.DateOfBirth <= _dateTimeProvider.Now.AddYears(-AgeOverThreshold);
    }
}