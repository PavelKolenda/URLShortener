namespace URLShortener.DTO;

public class ShortenedUrlUpdateDto
{
    public required string LongUrl { get; init; } = null!;
}