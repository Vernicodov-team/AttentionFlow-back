using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("api/v1/auth")]

public class AuthController: ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync()
    {
        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync()
    {
        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        return Ok();
    }
}