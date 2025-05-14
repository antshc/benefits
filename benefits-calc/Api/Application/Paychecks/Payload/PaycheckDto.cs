using Api.Domain.Paychecks;

namespace Api.Application.Paychecks.Payload;

public class PaycheckDto
{
    public decimal GrossAmount { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetAmount { get; set; }
    public PayPeriodType PayPeriodType { get; set; }
    public string PayPeriodTypeFriendlyName { get; set; }
    public int EmployeeId { get; set; }
    public List<DeductionLineDto> DeductionBreakdown { get; set; } = new();
}