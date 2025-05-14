using Api.Application.Paychecks.Payload;
using Api.Domain.Paychecks;

namespace Api.Application.Paychecks.Services;

public interface IPaychecksService
{
    Task<PaycheckDto> CreatePaycheck(int employeeId, short payPeriodType, CancellationToken cancellationToken);
}