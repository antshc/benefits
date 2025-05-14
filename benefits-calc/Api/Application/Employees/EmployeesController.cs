using Api.Application.Employees.Payload;
using Api.Application.Employees.Queries;
using Api.SharedKernel.Payload;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Application.Employees;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeQuery _employeeQuery;

    public EmployeesController(IEmployeeQuery employeeQuery)
    {
        _employeeQuery = employeeQuery;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id, CancellationToken cancellationToken)
    {
        try
        {
            var emp = await _employeeQuery.GetById(id, cancellationToken);

            var result = new ApiResponse<GetEmployeeDto>
            {
                Data = emp,
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

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<GetEmployeeDto>>>> GetAll(CancellationToken cancellationToken)
    {
        //task: use a more realistic production approach
        IReadOnlyCollection<GetEmployeeDto> employees = await _employeeQuery.GetAll(cancellationToken);

        var result = new ApiResponse<IReadOnlyCollection<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }
}