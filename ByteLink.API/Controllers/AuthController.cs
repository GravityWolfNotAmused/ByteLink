using ByteLink.Application.Mediator.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteLink.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            await mediator.Send(command);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var loginCommandResult = await mediator.Send(command);
            return Ok(loginCommandResult);
        }
        catch
        {
            return Unauthorized("Invalid Login.");
        }
    }
}
