using Api.Application.Paychecks.Payload;
using Api.Domain.Paychecks;

namespace Api.Application.Paychecks.Mapping;

internal interface IPaycheckMapper
{
    PaycheckDto Map(Paycheck paycheck);
}