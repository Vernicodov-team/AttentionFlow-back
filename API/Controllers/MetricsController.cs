using Microsoft.AspNetCore.Mvc;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("/metrics")]

public class MetricsController: ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetMetricsAsync()
    {
        return Ok();
    }
    
}