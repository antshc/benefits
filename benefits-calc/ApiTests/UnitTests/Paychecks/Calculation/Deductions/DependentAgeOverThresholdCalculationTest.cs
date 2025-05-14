using System;
using Api.Domain.Employees;
using Api.SharedKernel;
using Moq;
using Xunit;

namespace ApiTests.UnitTests.Paychecks.Calculation.Deductions;

public class DependentAgeOverThresholdCalculationTest : DeductionCalculationBaseTest
{
    private const int AgeThresholdInYears = 50;
    private readonly DateTime Today = new DateTime(2025, 05, 01);

    private readonly Employee TestEmployee = new Employee(
        "James",
        "LeBron",
        80000m,
        new DateTime(1984, 12, 30));

    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    public DependentAgeOverThresholdCalculationTest()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>(MockBehavior.Strict);
        _dateTimeProviderMock.Setup(d => d.Now).Returns(Today);
    }

    [Fact]
    public void WhenEmployeeHasSpouseOverAgeThreshold_ShouldDeductCalculatedAmount()
    {
        // Arrange
        TestEmployee.AddSpouse("Spouse", "LeBron", Today.AddYears(-AgeThresholdInYears));

        // Act
        var actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy",  276.92m);
        actual.AssertDeductionAmount("DependentAgeOverThresholdDeductionPolicy", 92.31m);
        actual.AssertPaycheck(
            grossAmount: 3076.92m,
            totalDeductions: 830.77m,
            netAmount: 2246.15m,
            BiWeeklyPeriodType.Type);
    }

    [Fact]
    public void WhenEmployeeHasDomesticPartnerOverAgeThreshold_ShouldDeductCalculatedAmount()
    {
        // Arrange
        TestEmployee.AddDomesticPartner("DomesticPartner", "LeBron", Today.AddYears(-AgeThresholdInYears));

        // Act
        var actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy",  276.92m);
        actual.AssertDeductionAmount("DependentAgeOverThresholdDeductionPolicy", 92.31m);
        actual.AssertPaycheck(
            grossAmount: 3076.92m,
            totalDeductions: 830.77m,
            netAmount: 2246.15m,
            BiWeeklyPeriodType.Type);
    }

    [Fact]
    public void WhenEmployeeHasChildUnderAgeThreshold_ShouldNotDeductAgeOverThresholdAmount()
    {
        // Arrange
        TestEmployee.AddChild("Child1", "LeBron", Today.AddYears(-20));

        // Act
        var actual = PaycheckCalculator.Calculate(TestEmployee, BiWeeklyPeriodType);

        // Assert
        actual.AssertDeductionAmount("BaseCostForBenefitsDeductionPolicy",  461.54m);
        actual.AssertDeductionAmount("AdditionalCostPerDependentForBenefitsDeductionPolicy",  276.92m);
        actual.AssertDeductionAmount("DependentAgeOverThresholdDeductionPolicy", 0.00m);
        actual.AssertPaycheck(
            grossAmount: 3076.92m,
            totalDeductions: 738.46m,
            netAmount: 2338.46m,
            BiWeeklyPeriodType.Type);
    }
}