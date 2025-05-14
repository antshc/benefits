using System;
using Api.Domain.Employees;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation.Deductions;

public class SalaryThresholdCalculationTest : DeductionCalculationBaseTest
{
    [Theory]
    [InlineData(80000, 3076.92, 461.54, 2615.38, 0)]
    [InlineData(80000.5, 3076.94, 523.08, 2553.86, 61.54)]
    public void WhenEmployeeSalaryThresholdCriteria_ShouldDeductCalculatedAmountCorrectly(double grossSalary, double expectedGross, double expectedTotalDeductions, double expectedNetAmount, double expectedSalaryThresholdDeduction)
    {
        // Arrange
        var employeeTestData = new Employee(
            "LeBron",
            "James",
            (decimal)grossSalary,
            new DateTime(1984, 12, 30));

        // Act
        var actual = PaycheckCalculator.Calculate(employeeTestData, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("SalaryThresholdDeductionPolicy", (decimal)expectedSalaryThresholdDeduction);
        actual.AssertPaycheck(
            grossAmount: (decimal)expectedGross,
            totalDeductions: (decimal)expectedTotalDeductions,
            netAmount: (decimal)expectedNetAmount,
            BiWeeklyPeriodType.Type);
    }
}