using Microsoft.AspNetCore.Mvc;
using URLShortener.DTO;
using URLShortener.Models;
using URLShortener.Services.Interfaces;

namespace URLShortener.Controllers;
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly IUrlManagementService _urlManagementService;

    public UrlShortenerController(IUrlService urlService, IUrlManagementService urlManagementService)
    {
        _urlService = urlService;
        _urlManagementService = urlManagementService;
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> Shorten([FromBody] UrlShortenRequest request)
    {
        var shortenedUrl = await _urlManagementService.CreateShortUrlAsync(request, HttpContext);

        return Ok(shortenedUrl);
    }

    [HttpGet("{code:length(6)}")]
    public async Task<IActionResult> RedirectToLongUrl(string code)
    {
        string longUrl = await _urlManagementService.GetLongUrlAsync(code);

        return Redirect(longUrl);
    }

    [HttpGet("list")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _urlService.GetAsync());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _urlService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ShortenedUrlUpdateDto shortenedUrlDto)
    {
        await _urlService.UpdateAsync(id, shortenedUrlDto);
        return NoContent();
    }
}
