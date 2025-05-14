// csharp

using System;
using System.Collections.Generic;
using Api.Domain.Paychecks;
using Xunit;

namespace ApiTests.UnitTests.Paychecks;

public class PaycheckUnitTest
{
    [Fact]
    public void WhenValidPaycheckIsCreated_CalculatesCorrectAmounts()
    {
        // Arrange
        decimal grossSalary = 1000m;
        var payPeriod = new PaycheckPeriod(PayPeriodType.Monthly);
        var annualDeductions = new List<DeductionLine>
        {
            new DeductionLine("Tax", 120m),
            new DeductionLine("Insurance", 50m)
        };
        int employeeId = 1;

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
    public void WhenPayPeriodIsBiWeekly_ThenGrossAmountCalculatedCorrectly()
    {
        decimal annualSalary = 52000m;
        var payPeriod = new PaycheckPeriod(PayPeriodType.BiWeekly);
        int employeeId = 123;
        var paycheck = new Paycheck(annualSalary, payPeriod, new List<DeductionLine>(), employeeId);
        paycheck.AssertPaycheck(annualSalary, new List<DeductionLine>(), payPeriod, employeeId);
    }

    [Fact]
    public void WhenNoDeductionsProvided_ThenDeductionBreakdownIsEmpty()
    {
        var paycheck = new Paycheck(26000m, new PaycheckPeriod(PayPeriodType.BiWeekly), new List<DeductionLine>(), 999);
        paycheck.AssertPaycheck(26000m, new List<DeductionLine>(), paycheck.PayPeriod, 999);
    }
}