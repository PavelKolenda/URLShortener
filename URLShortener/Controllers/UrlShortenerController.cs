using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;
using URLShortener.Services.Interfaces;

namespace URLShortener.Controllers;

[ApiController]
[Route("")]
public class UrlShortenerController(IUrlShortenerService urlShortenerService) : ControllerBase
{
    [HttpPost("shorten")]
    public async Task<IActionResult> Shorten([FromBody] UrlShortenRequest request)
    {
        ShortenedUrl shortenedUrl = await urlShortenerService.CreateShortUrlAsync(request, HttpContext);

        return Ok(shortenedUrl);
    }

    [HttpGet("{code:length(6)}")]
    public async Task<IActionResult> RedirectToLongUrl(string code)
    {
        string longUrl = await urlShortenerService.GetLongUrlAsync(code);

        return Redirect(longUrl);
    }
}