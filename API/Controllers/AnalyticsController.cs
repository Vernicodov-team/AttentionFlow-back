using Microsoft.AspNetCore.Mvc;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("api/v1/analytics")]

public class AnalyticsController: ControllerBase
{

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummaryAsync()
    {
        return Ok();
    }
    
}