using Microsoft.EntityFrameworkCore;
using MySqlConnector.Logging;
using URLShortener.Models;

namespace URLShortener.Repository;
public class UrlShortRepository : IUrlShortRepository
{
    private readonly AppDbContext _context;

    public UrlShortRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ShortenedUrl shortenedUrl)
    {
        await _context.ShortenedUrls.AddAsync(shortenedUrl);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsShortUrlUnique(string shortUrl)
    {
        return !await _context.ShortenedUrls.AnyAsync(s => s.ShortUrl == shortUrl);
    }

    public async Task<ShortenedUrl> GetUrl(string shortCode)
    {
        var shortenedUrl = await _context.ShortenedUrls
            .FirstOrDefaultAsync(s => s.ShortUrl.Substring(s.ShortUrl.Length - 6)
            .Equals(shortCode)) 
            ??
            throw new Exception($"Short URL was not found.");

        shortenedUrl.ClickCount++;
        await _context.SaveChangesAsync();

        return shortenedUrl;
    } 
}
