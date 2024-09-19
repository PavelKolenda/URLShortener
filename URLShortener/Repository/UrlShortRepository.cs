using Microsoft.EntityFrameworkCore;
using URLShortener.Models;
using URLShortener.Models.Exceptions;

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
    public async Task<List<ShortenedUrl>> GetAsync()
    {
        return await _context.ShortenedUrls.ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _context.ShortenedUrls.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> IsShortUrlUniqueAsync(string shortUrl)
    {
        return !await _context.ShortenedUrls.AnyAsync(s => s.ShortUrl == shortUrl);
    }

    public async Task<ShortenedUrl> GetUrlAsync(string shortUrl)
    {
        var shortenedUrl = await _context.ShortenedUrls
            .FirstOrDefaultAsync(s => s.ShortUrl.EndsWith(shortUrl))
            ??
            throw new NotFoundException($"Short URL wasn't found.");

        shortenedUrl.ClickCount++;
        await _context.SaveChangesAsync();

        return shortenedUrl;
    }

    public async Task UpdateAsync(ShortenedUrl shortenedUrl)
    {
        _context.ShortenedUrls.Update(shortenedUrl);
        await _context.SaveChangesAsync();
    }

    public async Task<ShortenedUrl> GetByIdAsync(int id)
    {
        return await _context.ShortenedUrls.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Short URL with {id} wasn't found");
    }

}
