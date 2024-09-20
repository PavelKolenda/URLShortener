using Microsoft.Extensions.Options;
using URLShortener.Options;
using URLShortener.Repository;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;
public class UrlShorteningService : IUrlShorteningService
{
    private readonly UrlShorteningOptions _options;
    private readonly Random _random = new Random();
    private readonly IUrlShortRepository _shortUrlRepository;

    public UrlShorteningService(IUrlShortRepository shortUrlRepository, IOptions<UrlShorteningOptions> options)
    {
        _shortUrlRepository = shortUrlRepository;
        _options = options.Value;
    }

    public async Task<string> Generate()
    {
        while (true)
        {
            char[] code = new char[_options.NumberOfCharsInShortUrl];
            for (int i = 0; i < _options.NumberOfCharsInShortUrl; i++)
            {
                int index = _random.Next(_options.Chars.Length - 1);
                code[i] = _options.Chars[index];
            }

            string shortUrl = new(code);

            if (await _shortUrlRepository.IsShortUrlUniqueAsync(shortUrl))
            {
                return shortUrl;
            }
        }
    }
}
