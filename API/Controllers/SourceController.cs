using Application.DTO;
using Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlowAnalis.Controllers;

[ApiController]
[Route("api/v1/source")]

public class SourceController: ControllerBase
{
    private readonly ISnapshotService _snapshotService;

    public SourceController(ISnapshotService snapshotService)
    {
        _snapshotService = snapshotService;
    }

    [HttpPut("snapshot")]
    [RequestSizeLimit(64L * 1024 * 1024)]
    public async Task<IActionResult> PutSnapshotAsync(CancellationToken cancellationToken)
    {
        try
        {
            SnapshotImportResultDto result;
            string contentType = Request.ContentType ?? string.Empty;

            if (contentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            {
                IFormCollection form = await Request.ReadFormAsync(cancellationToken);
                IFormFile? file = form.Files.FirstOrDefault();

                if (file is null || file.Length == 0)
                    return BadRequest("SnapshotFileIsMissing");

                await using Stream fileStream = file.OpenReadStream();
                result = await _snapshotService.ReplaceFromJsonAsync(fileStream, cancellationToken);
            }
            else
            {
                result = await _snapshotService.ReplaceFromJsonAsync(Request.Body, cancellationToken);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
