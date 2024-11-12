using URLShortener.DTO;
using URLShortener.Extensions.Pagination;
using URLShortener.Models;

namespace URLShortener.Services.Interfaces;

public interface IUrlService
{
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, ShortenedUrlUpdateDto shortenedUrlDto);
    Task<PagedList<ShortenedUrl>> GetPagedAsync(PagedParams pagedParams);
}