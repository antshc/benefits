using Api.Application.Paychecks.Mapping;
using Api.Application.Paychecks.Payload;
using Api.Domain.Employees;
using Api.Domain.Paychecks;
using Api.Domain.Paychecks.Calculation;

namespace Api.Application.Paychecks.Services;

// why? Encapsulates the use case of generating and retrieving paychecks.
// responsible for orchestrating workflows and preparing data for output
internal class PaychecksService : IPaychecksService
{
    private readonly IPaycheckCalculator _paycheckCalculator;
    private readonly IPaycheckMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;

    public PaychecksService(IPaycheckCalculator paycheckCalculator, IPaycheckMapper mapper, IEmployeeRepository employeeRepository)
    {
        _paycheckCalculator = paycheckCalculator;
        _mapper = mapper;
        _employeeRepository = employeeRepository;
    }

    public async Task<PaycheckDto> CreatePaycheck(int employeeId, short payPeriodType, CancellationToken cancellationToken)
    {
        Employee? emp = await _employeeRepository.GetById(employeeId, cancellationToken);

        if (emp is null) throw new ApplicationException("Employee not found");

        // why? Add mapping to period type with validation
        var periodType = (PayPeriodType)payPeriodType;
        var paycheck = _paycheckCalculator.Calculate(emp, new PaycheckPeriod(periodType));

        return _mapper.Map(paycheck);
    }
}