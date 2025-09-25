using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteLink.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Running");
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUser()
        {
            return Ok("Authed");
        }
    }
}
