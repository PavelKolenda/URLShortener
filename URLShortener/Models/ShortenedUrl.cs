using System.ComponentModel.DataAnnotations;

namespace URLShortener.Models;
public class ShortenedUrl
{
    public int Id { get; init; }
    [StringLength(1000)]
    public required string LongUrl { get; set; }
    [StringLength(35)]
    public required string ShortUrl { get; init; }
    public DateTime CreatedAt { get; init; }
    public int ClickCount { get; set; }
}
