using Microsoft.AspNetCore.Mvc;
using URLShortener.DTO;
using URLShortener.Services.Interfaces;

namespace URLShortener.Controllers;

[ApiController]
[Route("")]
public class UrlManagementController(IUrlService urlService) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> Get()
    {
        return Ok(await urlService.GetAsync());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await urlService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ShortenedUrlUpdateDto shortenedUrlDto)
    {
        await urlService.UpdateAsync(id, shortenedUrlDto);
        return NoContent();
    }
}