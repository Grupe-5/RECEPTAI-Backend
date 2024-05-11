using Microsoft.AspNetCore.Mvc;

namespace receptai.api.Controllers;

[Route("api/image")]
[ApiController]
public class ImageController(IImageService imageService) : ControllerBase
{
    private readonly IImageService _imageService = imageService;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var bytes = await _imageService.FetchImageAsync(id);
        if (bytes == null)
        {
            return NotFound();
        }

        return File(bytes, "image/jpeg");
    }
}
