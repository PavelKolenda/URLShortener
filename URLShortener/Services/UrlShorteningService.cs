using Microsoft.Extensions.Options;
using URLShortener.Options;
using URLShortener.Repository.Interfaces;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;

public class UrlShorteningService(IUrlShortRepository shortUrlRepository, IOptions<UrlShorteningOptions> options)
    : IUrlShorteningService
{
    private readonly UrlShorteningOptions _options = options.Value;
    private readonly Random _random = new();

    public async Task<string> GenerateAsync()
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

            if (await shortUrlRepository.IsShortUrlUniqueAsync(shortUrl))
            {
                return shortUrl;
            }
        }
    }
}