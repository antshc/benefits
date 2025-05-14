using System;
using System.Collections.Generic;
using Api.Domain.Paychecks;
using Xunit;
using System.Linq;

namespace ApiTests.UnitTests.Paychecks;

// Doesn't make sense to review, chat gpt generate this code
public static class ShouldExtensions
{
    public static void AssertPaycheck(this Paycheck paycheck, decimal grossSalary,
        IReadOnlyCollection<DeductionLine> annualDeductions, PaycheckPeriod expectedPayPeriod, int expectedEmployeeId)
    {
        var periodsPerYear = expectedPayPeriod.PaymentsPerYear;
        var expectedGross = decimal.Round(grossSalary / periodsPerYear, 2, MidpointRounding.AwayFromZero);
        var expectedBreakdown = annualDeductions?
            .Select(d => new
            {
                d.Type,
                Amount = decimal.Round(d.Amount / periodsPerYear, 2, MidpointRounding.AwayFromZero)
            })
            .ToList();
        var expectedTotalDeductions = expectedBreakdown.Sum(b => b.Amount);
        var expectedNet = expectedGross - expectedTotalDeductions;
        if (expectedNet < 0)
            expectedNet = 0;

        Assert.Equal(expectedGross, paycheck.GrossAmount);
        Assert.Equal(expectedTotalDeductions, paycheck.TotalDeductions);
        Assert.Equal(expectedNet, paycheck.NetAmount);
        Assert.Equal(expectedPayPeriod, paycheck.PayPeriod);
        Assert.Equal(expectedEmployeeId, paycheck.EmployeeId);

        if (annualDeductions != null)
        {
            Assert.Equal(expectedBreakdown.Count, paycheck.DeductionBreakdown.Count);
            foreach (var expected in expectedBreakdown)
            {
                paycheck.AssertHaveDeduction(expected.Type, expected.Amount);
            }
        }
    }

    public static void AssertHaveDeduction(this Paycheck paycheck, string expectedType, decimal expectedAmount)
    {
        var deduction = paycheck.DeductionBreakdown.FirstOrDefault(d => d.Type == expectedType);
        Assert.NotNull(deduction);
        Assert.Equal(expectedAmount, deduction.Amount);
    }
}