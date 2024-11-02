using Microsoft.EntityFrameworkCore;
using URLShortener.Models;
using URLShortener.Models.Exceptions;
using URLShortener.Repository.Interfaces;

namespace URLShortener.Repository;

public class UrlShortRepository(
    AppDbContext context, 
    ILogger<UrlShortRepository> logger) : IUrlShortRepository
{
    public async Task AddAsync(ShortenedUrl shortenedUrl)
    {
        await context.ShortenedUrls.AddAsync(shortenedUrl);
        await context.SaveChangesAsync();
        logger.LogInformation("Add url @{ShortenedUrl}", shortenedUrl);
    }

    public async Task<List<ShortenedUrl>> GetAsync()
    {
        logger.LogInformation("Get urls from database");
        return await context.ShortenedUrls.ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Delete url with id:@{Id}", id);
        await context.ShortenedUrls
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> IsShortUrlUniqueAsync(string shortUrl)
    {
        return !await context.ShortenedUrls.AnyAsync(s => s.ShortUrl == shortUrl);
    }

    public async Task<ShortenedUrl> GetUrlAsync(string shortUrl)
    {
        ShortenedUrl shortenedUrl = await context.ShortenedUrls
                                        .FirstOrDefaultAsync(s => s.ShortUrl.EndsWith(shortUrl))
                                    ??
                                    throw new NotFoundException("Short URL wasn't found.");

        shortenedUrl.ClickCount++;
        await context.SaveChangesAsync();
        logger.LogInformation("Get url @{ShortUrl}", shortUrl);

        return shortenedUrl;
    }

    public async Task UpdateAsync(ShortenedUrl shortenedUrl)
    {
        context.ShortenedUrls.Update(shortenedUrl);
        logger.LogInformation("Update url @{ShortenedUrl}", shortenedUrl);
        await context.SaveChangesAsync();
    }

    public async Task<ShortenedUrl> GetByIdAsync(int id)
    {
        return await context.ShortenedUrls.FirstOrDefaultAsync(x => x.Id == id)
               ?? throw new NotFoundException($"Short URL with {id} wasn't found");
    }
}