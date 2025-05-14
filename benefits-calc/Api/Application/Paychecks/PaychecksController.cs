using Api.Application.Employees.Payload;
using Api.Application.Paychecks.Payload;
using Api.Application.Paychecks.Services;
using Api.SharedKernel.Payload;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Application.Paychecks;

[ApiController]
[Route("api/v1/employees")]
public class PaychecksController : ControllerBase
{
    private readonly IPaychecksService _paychecksService;

    public PaychecksController(IPaychecksService paychecksService)
    {
        _paychecksService = paychecksService;
    }

    [SwaggerOperation(Summary = "Calculate paycheck for employee by id")]
    [HttpGet("{employeeId}/paychecks")]
    public async Task<ActionResult<ApiResponse<PaycheckDto>>> Get(int employeeId, [FromQuery] short periodtype = 26, CancellationToken cancellationToken = default)
    {
        try
        {
            var paycheck = await _paychecksService.CreatePaycheck(employeeId, periodtype, cancellationToken);

            var result = new ApiResponse<PaycheckDto>
            {
                Data = paycheck,
                Success = true
            };
            return result;
        }
        catch (ApplicationException e) when (e.Message == "Employee not found")
        {
            return NotFound(new ApiResponse<GetEmployeeDto>
            {
                Success = false,
                Message = e.Message
            });
        }
    }
}