using Api.Application.Dependents.Queries;
using Api.Application.Employees.Queries;
using Api.Application.Paychecks.Mapping;
using Api.Application.Paychecks.Services;
using Api.Data;
using Api.Domain.Paychecks;
using Api.Domain.Paychecks.Calculation;
using Api.Domain.Paychecks.Deductions;
using Api.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyRegistrations(this IServiceCollection services, IConfiguration configuration)
    {
        ApplicationLayer(services);
        DomainLayer(services);
        DataLayer(services, configuration);
        SharedKernel(services);

        return services;
    }

    private static void SharedKernel(IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    private static void DataLayer(IServiceCollection services, IConfiguration configuration)
    {
        // why? Store Connection strings and credentials securely (e.g., secrets, config providers).
        services.AddDbContext<BenefitsContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void ApplicationLayer(IServiceCollection services)
    {
        services.AddSingleton<IPaycheckMapper, PaycheckMapper>();
        services.AddScoped<IPaychecksService, PaychecksService>();
        services.AddScoped<IEmployeeQuery, EmployeeQuery>();
        services.AddScoped<IDependentQuery, DependentQuery>();
    }

    private static void DomainLayer(IServiceCollection services)
    {
        services.AddSingleton<IPaycheckCalculator, PaycheckCalculator>();
        services.AddSingleton<IDeductionPolicy, AdditionalCostPerDependentForBenefitsDeductionPolicy>();
        services.AddSingleton<IDeductionPolicy, BaseCostForBenefitsDeductionPolicy>();
        services.AddSingleton<IDeductionPolicy, SalaryThresholdDeductionPolicy>();
        services.AddSingleton<IDeductionPolicy, DependentAgeOverThresholdDeductionPolicy>();
    }
}