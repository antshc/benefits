namespace Api.Domain.Paychecks;

public class PaycheckPeriod
{
    public PaycheckPeriod(PayPeriodType type)
    {
        Type = type;
        PeriodPerYear = (short)type;
    }

    public short PeriodPerYear { get; }
    public PayPeriodType Type { get; }
}