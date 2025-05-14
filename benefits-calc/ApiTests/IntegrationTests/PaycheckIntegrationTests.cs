using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Application.Paychecks.Payload;
using Api.Domain.Paychecks;
using Xunit;

namespace ApiTests.IntegrationTests;

// why? The correctness of paycheck calculations is ensured through unit tests for the PaycheckCalculator, which cover all relevant test cases.
// Integration tests are intended to validate API correctness and component integration.
// While integration test coverage may be lower than that of unit tests, they should still cover the most critical scenarios.
public class PaycheckIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForAPaycheck_ShouldReturnACalculatedPaycheck()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1/paychecks?period=26");
        var expectedPaycheck = new PaycheckDto()
        {
            GrossAmount = 2900.81m,
            TotalDeductions = 461.54m,
            NetAmount = 2439.27m,
            PayPeriodType = (PayPeriodType)26,
            PayPeriodTypeFriendlyName = "Bi-Weekly",
            EmployeeId = 1,
            DeductionBreakdown = CreateDeductionBreakdown(
                dependentBenefitCost: 0m,
                baseBenefitCost: 461.54m,
                highIncomeSurchargeCost: 0m,
                dependentAgePlusCost: 0m)
        };
        await response.ShouldReturn(HttpStatusCode.OK, expectedPaycheck);
    }

    // why? Simple solution to reduce test method size and make test more readable
    private static List<DeductionLineDto> CreateDeductionBreakdown(
        decimal dependentBenefitCost,
        decimal baseBenefitCost,
        decimal highIncomeSurchargeCost,
        decimal dependentAgePlusCost)
    {
        return new List<DeductionLineDto>()
        {
            new DeductionLineDto()
            {
                Name = "Dependent Benefit Cost",
                Amount = dependentBenefitCost
            },
            new DeductionLineDto()
            {
                Name = "Employee Base Benefit Cost",
                Amount = baseBenefitCost
            },
            new DeductionLineDto()
            {
                Name = "High-Income Surcharge (Over $80K)",
                Amount = highIncomeSurchargeCost
            },
            new DeductionLineDto()
            {
                Name = "Additional Cost for Dependent Age 50+",
                Amount = dependentAgePlusCost
            }
        };
    }

    [Fact]
    public async Task WhenAskedForAPaycheckForMonthlyPeriodType_ShouldReturnACalculatedPaycheckMonthly()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1/paychecks?periodtype=12");
        var expectedPaycheck = new PaycheckDto()
        {
            GrossAmount = 6285.08m,
            TotalDeductions = 1000m,
            NetAmount = 5285.08m,
            PayPeriodType = (PayPeriodType)12,
            PayPeriodTypeFriendlyName = "Monthly",
            EmployeeId = 1,
            DeductionBreakdown = CreateDeductionBreakdown(
                dependentBenefitCost: 0m,
                baseBenefitCost: 1000m,
                highIncomeSurchargeCost: 0m,
                dependentAgePlusCost: 0m)
        };

        await response.ShouldReturn(HttpStatusCode.OK, expectedPaycheck);
    }
}