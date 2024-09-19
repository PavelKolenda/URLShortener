using URLShortener.Models;
using URLShortener.Repository;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;

public class UrlManagementService : IUrlManagementService
{
    private readonly IUrlShorteningService _urlShorteningService;
    private readonly IUrlShortRepository _shortUrlRepository;
    private readonly IUrlValidationService _urlValidationService;
    public UrlManagementService(IUrlShorteningService urlShorteningService,
                      IUrlShortRepository shortUrlRepository,
                      IUrlValidationService urlValidationService)
    {
        _urlShorteningService = urlShorteningService;
        _shortUrlRepository = shortUrlRepository;
        _urlValidationService = urlValidationService;
    }
    public async Task<ShortenedUrl> CreateShortUrlAsync(UrlShortenRequest request, HttpContext httpContext)
    {
        _urlValidationService.ValidateUrl(request.LongUrl);

        string shortUrl = await _urlShorteningService.Generate();

        ShortenedUrl shortedUrl = new()
        {
            LongUrl = request.LongUrl,
            ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{shortUrl}",
            CreatedAt = DateTime.Now
        };

        await _shortUrlRepository.AddAsync(shortedUrl);

        return shortedUrl;
    }

    public async Task<string> GetLongUrlAsync(string shortUrl)
    {
        ShortenedUrl shortenedUrl = await _shortUrlRepository.GetUrlAsync(shortUrl);

        return shortenedUrl.LongUrl;
    }
}