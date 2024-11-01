namespace URLShortener.Models;

public class UrlShortenRequest
{
    public required string LongUrl { get; init; } = null!;
}