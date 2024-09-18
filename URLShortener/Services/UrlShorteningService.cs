using URLShortener.Repository;

namespace URLShortener.Services;
public class UrlShorteningService : IUrlShorteningService
{
    const int NumberOfCharsInShortUrl = 6;
    const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new Random();
    private readonly IUrlShortRepository _shortUrlRepository;

    public UrlShorteningService(IUrlShortRepository shortUrlRepository)
    {
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<string> Generate()
    {
        while (true)
        {
            char[] shortUrl = new char[NumberOfCharsInShortUrl];
            for (int i = 0; i < NumberOfCharsInShortUrl; i++)
            {
                int index = _random.Next(Chars.Length - 1);
                shortUrl[i] = Chars[index];
            }

            string code = new(shortUrl);

            if (await _shortUrlRepository.IsShortUrlUnique(code))
            {
                return code;
            }
        }
    }
}
