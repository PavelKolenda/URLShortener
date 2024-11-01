using URLShortener.Models;
using URLShortener.Repository.Interfaces;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;

public class UrlShortenerService(
    IUrlShorteningService urlShorteningService,
    IUrlShortRepository shortUrlRepository,
    IUrlValidationService urlValidationService)
    : IUrlShortenerService
{
    public async Task<ShortenedUrl> CreateShortUrlAsync(UrlShortenRequest request, HttpContext httpContext)
    {
        urlValidationService.ValidateUrl(request.LongUrl);

        string shortUrl = await urlShorteningService.Generate();

        ShortenedUrl shortedUrl = new()
        {
            LongUrl = request.LongUrl,
            ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{shortUrl}",
            CreatedAt = DateTime.Now
        };

        await shortUrlRepository.AddAsync(shortedUrl);

        return shortedUrl;
    }

    public async Task<string> GetLongUrlAsync(string shortUrl)
    {
        ShortenedUrl shortenedUrl = await shortUrlRepository.GetUrlAsync(shortUrl);

        return shortenedUrl.LongUrl;
    }
}