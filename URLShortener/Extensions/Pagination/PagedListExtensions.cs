using Microsoft.EntityFrameworkCore;

namespace URLShortener.Extensions.Pagination;

public static class PagedListExtensions
{
    public static async Task<PagedList<T>> CreatePagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        int totalCount = await source.CountAsync();

        List<T> items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<T>(items, pageNumber, pageSize, totalCount);
    }

    public static PagedList<T> CreatePagedListFromList<T>(List<T> source, int pageNumber, int pageSize)
    {
        int totalCount = source.Count;

        List<T> items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        return new PagedList<T>(items, pageNumber, pageSize, totalCount);
    }
}
