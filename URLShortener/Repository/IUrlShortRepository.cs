using URLShortener.Models;

namespace URLShortener.Repository;
public interface IUrlShortRepository
{
    Task AddAsync(ShortenedUrl shortenedUrl);
    Task<ShortenedUrl> GetUrl(string shortUrl);
    Task<bool> IsShortUrlUnique(string shortUrl);
}
