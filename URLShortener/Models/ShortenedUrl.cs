namespace URLShortener.Models;
public class ShortenedUrl
{
    public int Id { get; set; }
    public string LongUrl { get; set; }
    public string ShortUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ClickCount { get; set; }
}

