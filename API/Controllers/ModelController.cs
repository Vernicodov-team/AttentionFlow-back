using Microsoft.AspNetCore.Mvc;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("api/v1/model")]

public class ModelController: ControllerBase
{

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettingstAsync()
    {
        return Ok();
    }

    [HttpPatch("settings")]
    public async Task<IActionResult> SettingsPatchAsync()
    {
        return Ok();
    }
}