using Api.Domain.Employees;
using Api.SharedKernel;

namespace Api.Domain.Paychecks.Deductions;

public class DependentAgeOverThresholdDeductionPolicy : IDeductionPolicy
{
    private const int MonthsPerYear = 12;
    private readonly IDateTimeProvider _dateTimeProvider;
    private const decimal AdditionalCostForOlderDependents = 200m;

    public DependentAgeOverThresholdDeductionPolicy(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public DeductionLine Calculate(Employee employee)
    {
        decimal annualCost = 0;
        Dependent? spouseOrDomesticPartner = employee.Dependents.SingleOrDefault(d => d.IsSpouseOrDomesticPartner());
        // why? this logic probably related to Employee's grandparents, but for now the only place where it potentially fit is there
        if (spouseOrDomesticPartner is not null && spouseOrDomesticPartner.IsAgeOverThreshold(_dateTimeProvider.Now))
        {
            annualCost += AdditionalCostForOlderDependents * MonthsPerYear;
        }

        return new DeductionLine(nameof(DependentAgeOverThresholdDeductionPolicy), annualCost);
    }
}