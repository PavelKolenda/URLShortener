using URLShortener.DTO;
using URLShortener.Models;

namespace URLShortener.Services.Interfaces;
public interface IUrlService
{
    Task DeleteAsync(int id);
    Task<List<ShortenedUrl>> GetAsync();
    Task UpdateAsync(int id, ShortenedUrlUpdateDto shortenedUrlDto);
}
