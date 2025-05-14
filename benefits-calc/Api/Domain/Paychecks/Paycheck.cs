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
    public decimal GrossAmount { get; }
    public decimal TotalDeductions { get; }
    public decimal NetAmount { get; }
    public PayPeriodType PayPeriod { get; }
    public int EmployeeId { get; }

    private List<DeductionLine> _deductionBreakdown = new();
    public IReadOnlyCollection<DeductionLine> DeductionBreakdown => _deductionBreakdown.AsReadOnly();

    public Paycheck(decimal grossAmount, PayPeriodType payPeriodType, IEnumerable<DeductionLine> deductions, int employeeId)
    {
        if (grossAmount < 0)
            throw new ArgumentException("Gross salary cannot be negative.");
        GrossAmount = grossAmount;
        PayPeriod = payPeriodType;
        EmployeeId = employeeId;

        foreach (var deduction in deductions)
        {
            _deductionBreakdown.Add(deduction);
        }

        TotalDeductions = _deductionBreakdown.Sum(d => d.Amount);
        decimal netAmount = GrossAmount - TotalDeductions;
        NetAmount = netAmount < 0 ? 0 : netAmount;
    }
}