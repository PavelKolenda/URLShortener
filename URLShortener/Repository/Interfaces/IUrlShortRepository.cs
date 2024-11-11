using URLShortener.Extensions.Pagination;
using URLShortener.Models;

namespace URLShortener.Repository.Interfaces;

public interface IUrlShortRepository
{
    Task AddAsync(ShortenedUrl shortenedUrl);
    Task DeleteAsync(int id);
    Task<List<ShortenedUrl>> GetAsync();
    Task<PagedList<ShortenedUrl>> GetPagedAsync(PagedParams pagingParams);
    Task<ShortenedUrl> GetByIdAsync(int id);
    Task<ShortenedUrl> GetUrlAsync(string shortUrl);
    Task<bool> IsShortUrlUniqueAsync(string shortUrl);
    Task UpdateAsync(ShortenedUrl shortenedUrl);
}