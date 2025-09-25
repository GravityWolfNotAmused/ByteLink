using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain;
using ByteLink.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteLink.API.Controllers;

[Route("")]
[ApiController]
public class RootController(
    IMediator mediator
)
: ControllerBase
{
    [HttpGet("{userSqid}/{shortCode}")]
    public async Task<IActionResult> VisitUrl([FromRoute] string userSqid, [FromRoute] string shortCode)
    {
        try
        {
            var redirectUrl = await mediator.Send(new VisitUrlCommand(userSqid, shortCode));

            return Redirect(redirectUrl);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> VisitAnonymouseUrl([FromRoute] string shortCode)
    {
        return await VisitUrl(Constants.AnonymousUserId, shortCode);
    }
}