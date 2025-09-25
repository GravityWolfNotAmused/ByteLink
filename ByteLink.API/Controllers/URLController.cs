using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain.Exceptions;
using Enyim.Caching;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteLink.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class URLController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> ShortenUrl([FromBody] ShortenUrlCommand command)
    {
        try
        {
            var result = await mediator.Send(command);

            return CreatedAtAction(nameof(GetUrl), null, result);
        }
        catch (Exception ex)
        when (ex is ArgumentException || ex is FormatException)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetUrl([FromBody] GetUrlByShortCodeCommand command)
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
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteUrl([FromBody] DeleteUrlByShortCodeCommand command)
    {
        try
        {
            await mediator.Send(command);
            return NoContent();
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

    [HttpGet("visits")]
    public async Task<IActionResult> GetUrlVisits([FromBody] GetUrlVisitsCommand command)
    {
        try
        {
            var visits = await mediator.Send(command);

            return Ok(visits);
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

    [HttpGet("{userSquid}")]
    public async Task<IActionResult> GetUrls([FromRoute] string userSquid)
    {
        try
        {
            var urls = await mediator.Send(new GetUrlsCommand(userSquid));
            return Ok(urls);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
