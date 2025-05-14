// csharp

using System;
using System.Collections.Generic;
using Api.Domain.Paychecks;
using Xunit;

namespace ApiTests.UnitTests.Paychecks;

public class PaycheckUnitTest
{
    [Theory]
    [InlineData(1000, PayPeriodType.Monthly, 1, new object[] { "Tax", 120, "Insurance", 50 })]
    [InlineData(2000, PayPeriodType.BiWeekly, 2, new object[] { "Tax", 200 })]
    public void WhenValidPaycheckIsCreated_CalculatesCorrectAmounts(
        decimal grossSalary,
        PayPeriodType payPeriodType,
        int employeeId,
        object[] deductionData)
    {
        // Arrange
        var payPeriod = new PaycheckPeriod(payPeriodType);
        var annualDeductions = new List<DeductionLine>();
        for (int i = 0; i < deductionData.Length; i += 2)
        {
            annualDeductions.Add(new DeductionLine((string)deductionData[i], Convert.ToDecimal(deductionData[i + 1])));
        }

        // Act
        var paycheck = new Paycheck(grossSalary, payPeriod, annualDeductions, employeeId);

        // Assert using computed expected values inside the extension method
        paycheck.AssertPaycheck(grossSalary, annualDeductions, payPeriod, employeeId);
    }

    [Fact]
    public void WhenAnnualDeductionsAreNull_ThenThrowsArgumentNullException()
    {
        decimal grossSalary = 1000m;
        var payPeriod = new PaycheckPeriod(PayPeriodType.Monthly);
        IReadOnlyCollection<DeductionLine> annualDeductions = null;
        Assert.Throws<ArgumentNullException>(() => new Paycheck(grossSalary, payPeriod, annualDeductions, 1));
    }

    [Fact]
    public void WhenGrossSalaryIsNegative_ThenThrowsArgumentException()
    {
        decimal grossSalary = -100m;
        var payPeriod = new PaycheckPeriod(PayPeriodType.Monthly);
        var annualDeductions = new List<DeductionLine>
        {
            new DeductionLine("Tax", 120m)
        };
        Assert.Throws<ArgumentException>(() => new Paycheck(grossSalary, payPeriod, annualDeductions, 1));
    }

    [Fact]
    public void WhenDeductionsExceedGrossAmount_ThenNetAmountSetToZero()
    {
        decimal grossSalary = 1000m;
        var payPeriod = new PaycheckPeriod(PayPeriodType.Monthly);
        var annualDeductions = new List<DeductionLine>
        {
            new DeductionLine("Tax", 1000m)
        };

        var paycheck = new Paycheck(grossSalary, payPeriod, annualDeductions, 1);

        paycheck.AssertPaycheck(grossSalary, annualDeductions, payPeriod, 1);
    }

    [Fact]
    public void WhenNoDeductionsProvided_ThenDeductionBreakdownIsEmpty()
    {
        var paycheck = new Paycheck(26000m, new PaycheckPeriod(PayPeriodType.BiWeekly), new List<DeductionLine>(), 999);
        paycheck.AssertPaycheck(26000m, new List<DeductionLine>(), paycheck.PayPeriod, 999);
    }
}