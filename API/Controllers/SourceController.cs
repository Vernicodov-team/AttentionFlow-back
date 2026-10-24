using Microsoft.AspNetCore.Mvc;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("api/v1/source")]

public class SourceController: ControllerBase
{

    [HttpPut("snapshot")]
    public async Task<IActionResult> PutSnapshotAsync()
    {
        return Ok();
    }
    
}