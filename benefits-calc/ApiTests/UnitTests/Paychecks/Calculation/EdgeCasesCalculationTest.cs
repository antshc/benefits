using System;
using Api.Domain.Employees;
using Api.Domain.Paychecks;
using ApiTests.UnitTests.Paychecks.Calculation.Deductions;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation;

public class EdgeCasesCalculationTest : DeductionCalculationBaseTest
{
    [Fact]
    public void WhenEmployeeWithoutSalary_ShouldGenerateEmptyPaycheck()
    {
        // Arrange
        var employee = new Employee(
            "LeBron",
            "James",
            0.00m,
            new DateTime(1984, 12, 30));
        // Act
        var actual = PaycheckCalculator.Calculate(employee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy", 461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 0.00m);
        actual.AssertDeductionAmount("DependentAgeOverThresholdDeductionPolicy", 0.00m);
        actual.AssertDeductionAmount("SalaryThresholdDeductionPolicy", (decimal)0.00m);

        actual.AssertPaycheck(
            grossAmount: 0.00m,
            totalDeductions: 461.54m,
            netAmount: 0.00m,
            BiWeeklyPeriodType.Type);
    }

    [Fact]
    public void WhenEmployeeWithNegativeSalary_ShouldThrowsException()
    {
        // Arrange
        var employee = new Employee(
            "LeBron",
            "James",
            -1.00m,
            new DateTime(1984, 12, 30));

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var actual = PaycheckCalculator.Calculate(employee, BiWeeklyPeriodType);
        });
    }

    [Fact]
    public void WhenEmployeeHasManyDependents_ShouldGeneratePaycheck()
    {
        // Arrange
        var employee = new Employee(
            "James",
            "LeBron",
            78000m,
            new DateTime(1984, 12, 30));

        // Arrange
        employee.AddChild("Child1", "LeBron", new DateTime(2010, 1, 1));
        employee.AddChild("Child2", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child3", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child4", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child5", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child6", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child7", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child8", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child9", "LeBron", new DateTime(2011, 1, 1));
        employee.AddChild("Child10", "LeBron", new DateTime(2011, 1, 1));

        // Act
        Paycheck actual = PaycheckCalculator.Calculate(employee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy", 461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 2769.23m);

        actual.AssertPaycheck(
            grossAmount: 3000.00m,
            totalDeductions: 3230.77m,
            netAmount: 0.00m,
            BiWeeklyPeriodType.Type);
    }
}