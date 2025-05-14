using System;
using Api.Domain.Employees;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation.Deductions;

public class BaseCostPerMonthCalculationTest : DeductionCalculationBaseTest
{
    private readonly Employee EmployeeTestData = new Employee(
        "LeBron",
        "James",
        78000m,
        new DateTime(1984, 12, 30));

    [Fact]
    public void WhenEmployeeWithOnlyBaseCostDeduction_ShouldDeductCorrectlyForPeriod()
    {
        // Act
        var actual = PaycheckCalculator.Calculate(EmployeeTestData, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy", 461.54m);
        actual.AssertPaycheck(
            grossAmount: 3000.00m,
            totalDeductions: 461.54m,
            netAmount: 2538.46m,
            BiWeeklyPeriodType.Type);
    }
}