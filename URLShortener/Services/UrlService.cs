using URLShortener.DTO;
using URLShortener.Models;
using URLShortener.Repository;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;
public class UrlService : IUrlService
{
    private readonly IUrlShortRepository _shortUrlRepository;
    private readonly IUrlValidationService _urlValidationService;
    public UrlService(IUrlShortRepository shortUrlRepository,
                      IUrlValidationService urlValidationService)
    {
        _shortUrlRepository = shortUrlRepository;
        _urlValidationService = urlValidationService;
    }


    public async Task<List<ShortenedUrl>> GetAsync()
    {
        return await _shortUrlRepository.GetAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _shortUrlRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(int id, ShortenedUrlUpdateDto shortenedUrlDto)
    {
        _urlValidationService.ValidateUrl(shortenedUrlDto.LongUrl);

        ShortenedUrl shortenedUrl = await _shortUrlRepository.GetByIdAsync(id);
        shortenedUrl.LongUrl = shortenedUrlDto.LongUrl;

        await _shortUrlRepository.UpdateAsync(shortenedUrl);
    }
}