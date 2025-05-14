using Api.Application.Paychecks.Mapping;
using Api.Application.Paychecks.Payload;
using Api.Data;
using Api.Domain.Employees;
using Api.Domain.Paychecks;
using Api.Domain.Paychecks.Calculation;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Paychecks.Services;

// why? Encapsulates the use case of generating and retrieving paychecks.
// responsible for orchestrating workflows and preparing data for output
internal class PaychecksService : IPaychecksService
{
    private readonly IPaycheckCalculator _paycheckCalculator;
    private readonly IPaycheckMapper _mapper;
    private readonly BenefitsContext _context;

    public PaychecksService(IPaycheckCalculator paycheckCalculator, IPaycheckMapper mapper, BenefitsContext context)
    {
        _paycheckCalculator = paycheckCalculator;
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaycheckDto> CreatePaycheck(int employeeId, short payPeriodType, CancellationToken cancellationToken)
    {
        Employee? emp = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

        if (emp is null) throw new ApplicationException("Employee not found");

        // why? Add mapping to period type with validation
        var periodType = (PayPeriodType)payPeriodType;
        var paycheck = _paycheckCalculator.Calculate(emp, new PaycheckPeriod(periodType));

        return _mapper.Map(paycheck);
    }
}