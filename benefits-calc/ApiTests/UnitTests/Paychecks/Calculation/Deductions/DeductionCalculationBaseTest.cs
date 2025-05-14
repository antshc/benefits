using System;
using System.Collections.Generic;
using Api.Domain.Paychecks;
using Api.Domain.Paychecks.Calculation;
using Api.Domain.Paychecks.Deductions;
using Api.SharedKernel;
using Moq;

namespace ApiTests.UnitTests.Paychecks.Calculation.Deductions;

// why? The initial implementation placed all calculation logic inside PaycheckCalculator,
// which made it harder to maintain and extend as business rules evolved.
// To improve modularity and clarity, the logic was refactored into separate DeductionPolicy classes,
// each responsible for a specific business rule (e.g., base cost, dependent surcharge).
//
// PaycheckCalculator now serves as a coordinator, delegating logic to policies.
// Sociable unit tests validate the complete behavior and ensure the resulting Paycheck is correct and understandable.
// These tests remain reliable even if the internal implementation of PaycheckCalculator changes.
//
// The design could be further improved by introducing an abstraction for testing pay period types (BiWeekly and Monthly)
public abstract class DeductionCalculationBaseTest
{
    protected readonly DateTime Today = new DateTime(2025, 05, 01);
    protected readonly PaycheckPeriod BiWeeklyPeriodType = new PaycheckPeriod(PayPeriodType.BiWeekly);
    protected readonly PaycheckPeriod MonthlyPeriodType = new PaycheckPeriod(PayPeriodType.Monthly);
    protected readonly Mock<IDateTimeProvider> DateTimeProviderMock;

    protected readonly PaycheckCalculator PaycheckCalculator;
    protected DeductionCalculationBaseTest()
    {
        DateTimeProviderMock = new Mock<IDateTimeProvider>(MockBehavior.Strict);
        DateTimeProviderMock.Setup(d => d.Now).Returns(Today);
        PaycheckCalculator = new PaycheckCalculator(new List<IDeductionPolicy>()
        {
            new AdditionalCostPerDependentForBenefitsDeductionPolicy(),
            new BaseCostForBenefitsDeductionPolicy(),
            new DependentAgeOverThresholdDeductionPolicy(DateTimeProviderMock.Object),
            new SalaryThresholdDeductionPolicy()
        });
    }
}