using Api.Application.Paychecks.Payload;
using Api.Domain.Paychecks;
using Api.Domain.Paychecks.Deductions;

namespace Api.Application.Paychecks.Mapping;

// Why? Use auto mapper which simplifies object-to-object mapping and reduces boilerplate code.
internal class PaycheckMapper : IPaycheckMapper
{
    public PaycheckDto Map(Paycheck paycheck)
    {
        return new PaycheckDto
        {
            GrossAmount = paycheck.GrossAmount,
            TotalDeductions = paycheck.TotalDeductions,
            NetAmount = paycheck.NetAmount,
            PayPeriodType = paycheck.PayPeriod,
            PayPeriodTypeFriendlyName = ToFriendlyName(paycheck.PayPeriod),
            EmployeeId = paycheck.EmployeeId,
            DeductionBreakdown = paycheck.DeductionBreakdown
                .Select(d => new DeductionLineDto
                {
                    Amount = d.Amount,
                    Name = ToFriendlyName(d.Type)
                })
                .ToList()
        };
    }

    private string ToFriendlyName(PayPeriodType periodType)
    {
        return periodType switch
        {
            PayPeriodType.BiWeekly => "Bi-Weekly",
            PayPeriodType.Monthly => "Monthly",
            _ => periodType.ToString()
        };
    }

    private string ToFriendlyName(string type)
    {
        return type switch
        {
            nameof(AdditionalCostPerDependentForBenefitsDeductionPolicy) => "Dependent Benefit Cost",
            nameof(BaseCostForBenefitsDeductionPolicy) => "Employee Base Benefit Cost",
            nameof(DependentAgeOverThresholdDeductionPolicy) => "Additional Cost for Dependent Age 50+",
            nameof(SalaryThresholdDeductionPolicy) => "High-Income Surcharge (Over $80K)",
            _ => type
        };
    }
}