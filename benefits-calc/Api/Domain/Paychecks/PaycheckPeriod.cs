namespace Api.Domain.Paychecks;

public class PaycheckPeriod
{
    public PaycheckPeriod(PayPeriodType type)
    {
        Type = type;
        PaymentsPerYear = (short)type;
    }

    public short PaymentsPerYear { get; }
    public PayPeriodType Type { get; }
}