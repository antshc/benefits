using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Paychecks;
using Xunit;

namespace ApiTests.UnitTests.Paychecks;

public class PaycheckUnitTest
{
    [Fact]
    public void ArgumentExceptionIsThrownIfGrossAmountIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
            new Paycheck(-1m, PayPeriodType.BiWeekly, Enumerable.Empty<DeductionLine>(), 123));
    }

    [Fact]
    public void GrossAmountIsDividedByPeriodType()
    {
        var paycheck = new Paycheck(52000m, PayPeriodType.BiWeekly, Enumerable.Empty<DeductionLine>(), 123);
        Assert.Equal(2000m, paycheck.GrossAmount);
    }

    [Fact]
    public void BuildsWithoutDeductionsProperly()
    {
        var paycheck = new Paycheck(26000m, PayPeriodType.BiWeekly, new List<DeductionLine>(), 999);
        Assert.Empty(paycheck.DeductionBreakdown);
    }
}