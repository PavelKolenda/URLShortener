using URLShortener.DTO;
using URLShortener.Extensions.Pagination;
using URLShortener.Models;
using URLShortener.Repository.Interfaces;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;

public class UrlService(
    IUrlShortRepository shortUrlRepository,
    IUrlValidationService urlValidationService)
    : IUrlService
{
    public async Task<List<ShortenedUrl>> GetAsync()
    {
        return await shortUrlRepository.GetAsync();
    }

    public async Task<PagedList<ShortenedUrl>> GetPagedAsync(PagedParams pagedParams)
    {
        return await shortUrlRepository.GetPagedAsync(pagedParams);
    }

    public async Task DeleteAsync(int id)
    {
        await shortUrlRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(int id, ShortenedUrlUpdateDto shortenedUrlDto)
    {
        urlValidationService.ValidateUrl(shortenedUrlDto.LongUrl);

        ShortenedUrl shortenedUrl = await shortUrlRepository.GetByIdAsync(id);
        shortenedUrl.LongUrl = shortenedUrlDto.LongUrl;

        await shortUrlRepository.UpdateAsync(shortenedUrl);
    }
}