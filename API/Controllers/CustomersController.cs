using Microsoft.AspNetCore.Mvc;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("api/v1/customers")]

public class CustomersController: ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIdAsync(int id)
    {
        return Ok();
    }
}