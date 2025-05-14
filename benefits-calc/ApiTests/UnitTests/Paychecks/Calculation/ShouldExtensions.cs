using System;
using System.Linq;
using Api.Domain.Paychecks;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation;

internal static class ShouldExtensions
{
    internal static void AssertDeductionAmount(this Paycheck paycheck, string deductionType, decimal expectedAmount)
    {
        DeductionLine? actualDeduction = paycheck.DeductionBreakdown.FirstOrDefault(d => d.Type == deductionType);
        Assert.NotNull(actualDeduction);
        Assert.Equal(RoundToTwoDecimalPlaces(expectedAmount), actualDeduction?.Amount);
    }

    internal static void AssertPaycheck(
        this Paycheck paycheck,
        decimal grossAmount,
        decimal totalDeductions,
        decimal netAmount,
        PayPeriodType payPeriodType)
    {
        Assert.Equal(grossAmount, paycheck.GrossAmount);
        Assert.Equal(totalDeductions, paycheck.TotalDeductions);
        Assert.Equal(netAmount, paycheck.NetAmount);
        Assert.Equal(payPeriodType, payPeriodType);
    }

    internal static decimal RoundToTwoDecimalPlaces(this decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}