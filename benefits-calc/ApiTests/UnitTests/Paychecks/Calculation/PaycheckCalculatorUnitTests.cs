using System;
using System.Collections.Generic;
using Api.Domain.Employees;
using Api.Domain.Paychecks;
using Api.Domain.Paychecks.Calculation;
using Api.Domain.Paychecks.Deductions;
using Moq;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation;

public class PaycheckCalculatorUnitTests
{
    [Fact]
    public void WhenNoPolicies_ShouldReturnEmpty()
    {
        // Arrange
        var employee = new Employee(
            "LeBron",
            "James",
            75420.99m,
            new DateTime(1984, 12, 30));

        var policies = new List<IDeductionPolicy>();
        var paycheckPeriod = new PaycheckPeriod(PayPeriodType.BiWeekly);

        // Act
        var actual = new PaycheckCalculator(policies).Calculate(employee, paycheckPeriod);

        // Assert
        Assert.Empty(actual.DeductionBreakdown);
        Assert.Equal(0m, actual.TotalDeductions);
    }

    [Fact]
    public void WhenApplyBaseCostPerMonthBenefitPolicy_ShouldReturnTotalAmountWithOnlyBaseCostPerMonthBenefit()
    {
        // Arrange
        var employee = new Employee(
            "LeBron",
            "James",
            75420.99m,
            new DateTime(1984, 12, 30));

        var paycheckPeriod = new PaycheckPeriod(PayPeriodType.BiWeekly);
        var deductionPolicy1Mock = new Mock<IDeductionPolicy>();
        deductionPolicy1Mock.Setup(d => d.Calculate(employee)).Returns(new DeductionLine("DeductionPolicy1", 100m));
        var deductionPolicy2Mock = new Mock<IDeductionPolicy>();
        deductionPolicy2Mock.Setup(d => d.Calculate(employee)).Returns(new DeductionLine("DeductionPolicy2", 100m));
        var paycheckCalculator = new PaycheckCalculator(new List<IDeductionPolicy>()
        {
            deductionPolicy1Mock.Object,
            deductionPolicy2Mock.Object
        });

        // Act
        var paycheck = paycheckCalculator.Calculate(employee, paycheckPeriod);

        // Assert
        Assert.Equal(2, paycheck.DeductionBreakdown.Count);
        deductionPolicy1Mock.Verify(x => x.Calculate(employee), Times.Once);
        deductionPolicy2Mock.Verify(x => x.Calculate(employee), Times.Once);
    }
}