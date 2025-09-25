using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteLink.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class MetricsController(
    IMediator mediator
) : ControllerBase
{
    [HttpGet("visits")]
    public async Task<IActionResult> GetUrlTotalVisits([FromBody] GetTotalUrlVisitsCommand command)
    {
        try
        {
            var viewModel = await mediator.Send(command);

            return Ok(viewModel);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
