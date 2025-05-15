using System;
using Api.Domain.Employees;
using Api.Domain.Paychecks;
using ApiTests.UnitTests.Paychecks.Calculation.Deductions;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation;

public class EdgeCasesCalculationTest : DeductionCalculationBaseTest
{
    [Fact]
    public void WhenEmployeeHasAllDeductions_ShouldGenerateCompletePaycheck()
    {
        // Arrange
        var employee = new Employee(
            "Full",
            "Morant",
            92365.22m,
            new DateTime(1984, 12, 30));

        employee.AddDependent(new Dependent("Spouse1", "Morant", new DateTime(1974, 3, 3), Relationship.Spouse));
        employee.AddDependent(new Dependent("Child1", "Morant", new DateTime(2010, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child2", "Morant", new DateTime(2011, 1, 1), Relationship.Child));

        // Act
        var actual = PaycheckCalculator.Calculate(employee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy", 461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy", 830.77m);
        actual.AssertDeductionAmount("DependentAgeOverThresholdDeductionPolicy", 92.31m);
        actual.AssertDeductionAmount("SalaryThresholdDeductionPolicy", (decimal)71.05m);

        actual.AssertPaycheck(
            grossAmount: 3552.51m,
            totalDeductions: 1455.67m,
            netAmount: 2096.84m,
            BiWeeklyPeriodType.Type);
    }

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
    public void WhenEmployeeHasManyDependents_ShouldGeneratePaycheck()
    {
        // Arrange
        var employee = new Employee(
            "James",
            "LeBron",
            78000m,
            new DateTime(1984, 12, 30));

        // Arrange
        employee.AddDependent(new Dependent("Child1", "LeBron", new DateTime(2010, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child2", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child3", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child4", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child5", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child6", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child7", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child8", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child9", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));
        employee.AddDependent(new Dependent("Child10", "LeBron", new DateTime(2011, 1, 1), Relationship.Child));

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