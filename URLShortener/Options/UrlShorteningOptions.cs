namespace URLShortener.Options;

public class UrlShorteningOptions
{
    public int NumberOfCharsInShortUrl { get; init; }
    public required string Chars { get; init; }
}