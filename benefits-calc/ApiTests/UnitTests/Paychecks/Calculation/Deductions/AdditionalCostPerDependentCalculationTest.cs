using System;
using Api.Domain.Employees;
using Api.Domain.Paychecks;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation.Deductions;

public class AdditionalCostPerDependentCalculationTest : DeductionCalculationBaseTest
{
    private readonly Employee TestEmployee = new Employee(
        "James",
        "LeBron",
        78000m,
        new DateTime(1984, 12, 30));

    [Fact]
    public void WhenEmployeeWithoutDependents_ShouldDeductZero()
    {
        // Act
        Paycheck actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 0.00m);
        actual.AssertPaycheck(
            grossAmount: 3000.00m,
            totalDeductions: 461.54m,
            netAmount: 2538.46m,
            BiWeeklyPeriodType.Type);
    }

    [Fact]
    public void WhenEmployeeWithOneChild_ShouldDeductCorrectly()
    {
        // Arrange
        TestEmployee.AddChild("ChildA", "LeBron", new DateTime(2015, 5, 5));

        // Act
        Paycheck actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 276.92m);
        actual.AssertPaycheck(
            grossAmount: 3000.00m,
            totalDeductions: 738.46m,
            netAmount: 2261.54m,
            BiWeeklyPeriodType.Type);
    }

    [Fact]
    public void EmployeeWithMultipleChildren_ShouldDeductCorrectly()
    {
        // Arrange
        TestEmployee.AddChild("Child1", "LeBron", new DateTime(2010, 1, 1));
        TestEmployee.AddChild("Child2", "LeBron", new DateTime(2011, 1, 1));

        // Act
        Paycheck actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 553.85m);
        actual.AssertPaycheck(
            grossAmount: 3000.00m,
            totalDeductions: 1015.39m,
            netAmount: 1984.61m,
            BiWeeklyPeriodType.Type);
    }

    [Fact]
    public void WhenEmployeeWithTwoChildrenAndPartner_ShouldDeductCorrectly()
    {
        // Arrange
        TestEmployee.AddChild("Child1", "LeBron", new DateTime(2010, 1, 1));
        TestEmployee.AddChild("Child2", "LeBron", new DateTime(2011, 1, 1));
        TestEmployee.AddDomesticPartner("Partner", "LeBron", new DateTime(2009, 6, 15));

        // Act
        Paycheck actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 830.77m);
        actual.AssertPaycheck(
            grossAmount: 3000.00m,
            totalDeductions: 1292.31m,
            netAmount: 1707.69m,
            BiWeeklyPeriodType.Type);
    }

    [Fact]
    public void WhenEmployeeWithTwoChildrenAndSpouse_ShouldDeductCorrectly()
    {
        // Arrange
        TestEmployee.AddChild("Child1", "LeBron", new DateTime(2010, 1, 1));
        TestEmployee.AddChild("Child2", "LeBron", new DateTime(2010, 1, 1));
        TestEmployee.AddSpouse("Partner", "LeBron", new DateTime(2010, 1, 1));

        // Act
        Paycheck actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 830.77m);
        actual.AssertPaycheck(
            grossAmount: 3000.00m,
            totalDeductions: 1292.31m,
            netAmount: 1707.69m,
            BiWeeklyPeriodType.Type);
    }
}