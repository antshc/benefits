namespace Api.Domain.Paychecks;

// why? Paycheck is looks more as domain entity not a value object, because:
// - It has a unique identity (EmployeeId + PayPeriodType)
// - In the future Paychecks may be managed independently of Employee, and fetch a paycheck by ID for reporting, audit, reissue, or display.
// - In the future it may include data beyond calculation (e.g., issue date, confirmation)
// - In the future it may be traceable for audit and reporting purposes
public class Paycheck
{
    // why? Decimal is more suitable for financial calculations due to its higher precision and ability to represent decimal fractions exactly, avoiding rounding errors that can occur with floating-point types like double.
    // It good to have Money Value Object. Using a Money value object ensures type safety, encapsulates monetary logic. It centralizes rounding and formatting rules.
    // In general is good to value objects for other primitive values to ensures type safety, but it requires carefull investigation what make a value object.
    public decimal GrossAmount { get; private set; }
    public decimal TotalDeductions { get; private set; }
    public decimal NetAmount { get; private set; }
    public PaycheckPeriod PayPeriod { get; private set; }
    public int EmployeeId { get; private set; }

    private List<DeductionLine> _deductionBreakdown = new();
    public IReadOnlyCollection<DeductionLine> DeductionBreakdown => _deductionBreakdown.AsReadOnly();

    public Paycheck(decimal grossSalary, PaycheckPeriod payPeriod, IReadOnlyCollection<DeductionLine> annualDeductions, int employeeId)
    {
        if (annualDeductions == null)
            throw new ArgumentNullException(nameof(annualDeductions));
        if (grossSalary < 0)
            throw new ArgumentException("Gross salary cannot be negative.");

        ApplyDeductionBreakdown(annualDeductions, payPeriod.PaymentsPerYear);

        GrossAmount = CalculateGrossAmount(grossSalary, payPeriod.PaymentsPerYear);
        PayPeriod = payPeriod;
        EmployeeId = employeeId;

        TotalDeductions = DeductionBreakdown.Sum(d => d.Amount);
        SetNetAmount(CalculateNetAmount());
    }

    private void ApplyDeductionBreakdown(IReadOnlyCollection<DeductionLine> annualDeductions, short periodsPerYear)
    {
        _deductionBreakdown = annualDeductions
            .Select(d => new DeductionLine(d.Type, RoundToTwoDecimalPlaces(d.Amount / periodsPerYear)))
            .ToList();
    }

    private decimal CalculateGrossAmount(decimal grossSalary, short periodsPerYear)
    {
        return RoundToTwoDecimalPlaces(grossSalary / periodsPerYear);
    }

    private decimal CalculateNetAmount()
    {
        return GrossAmount - TotalDeductions;
    }

    private void SetNetAmount(decimal netAmount)
    {
        if (netAmount > 0)
        {
            NetAmount = netAmount;
        }
    }

    private static decimal RoundToTwoDecimalPlaces(decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}