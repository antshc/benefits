using Api.Application.Dependents.Payload;
using Api.Application.Dependents.Queries;
using Api.SharedKernel.Payload;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Application.Dependents;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentQuery _dependentQuery;

    public DependentsController(IDependentQuery dependentQuery)
    {
        _dependentQuery = dependentQuery;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id, CancellationToken cancellationToken)
    {
        try
        {
            var dep = await _dependentQuery.GetById(id, cancellationToken);
            return new ApiResponse<GetDependentDto>
            {
                Data = dep,
                Success = true
            };
        }
        catch (ApplicationException e) when (e.Message == "Dependent not found")
        {
            return NotFound(new ApiResponse<GetDependentDto>
            {
                Success = false,
                Message = e.Message
            });
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var deps = await _dependentQuery.GetAll(cancellationToken);
        return new ApiResponse<List<GetDependentDto>>
        {
            Data = deps.ToList(),
            Success = true
        };
    }
}