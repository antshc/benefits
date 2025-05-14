namespace Api.Domain.Paychecks;

public class DeductionLine
{
    //  why? Do not use string as a type identifier
    public string Type { get; private set; }
    public decimal Amount { get; private set; }

    public DeductionLine(string type, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Deduction type is required.");

        if (amount < 0)
            throw new ArgumentException("Deduction amount cannot be negative.");

        Type = type;
        Amount = amount;
    }
}