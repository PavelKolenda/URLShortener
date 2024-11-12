using System.Text.RegularExpressions;
using URLShortener.Models.Exceptions;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;

public class UrlValidationService(IHttpContextAccessor httpContext) : IUrlValidationService
{
    public void ValidateUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) || !ValidateUrlWithRegex(url))
        {
            throw new InvalidUrlException($"The provided URL is not valid. Url: {url}");
        }

        if (IsUrlUsingHttp(uriResult))
        {
            throw new InvalidUrlException($"The URL must start with 'http' or 'https'. Url: {url}");
        }

        if (string.IsNullOrWhiteSpace(uriResult.Host))
        {
            throw new InvalidUrlException($"The URL must have a valid host. Url: {url}");
        }

        if (IsUrlShorten(uriResult))
        {
            throw new InvalidUrlException($"The provided URL is already shorten. Url: {url}");
        }
    }

    private static bool ValidateUrlWithRegex(string url)
    {
        Regex urlRegex = new(
            @"^(https?):\/\/(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}" +
            @"(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5]))?" +
            @"(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$",
            RegexOptions.IgnoreCase);

        return urlRegex.IsMatch(url);
    }

    private bool IsUrlShorten(Uri uri)
    {
        HttpContext context = httpContext.HttpContext
                              ?? throw new ArgumentNullException(nameof(httpContext));

        return uri.Scheme == context.Request.Scheme
               && uri.Host == context.Request.Host.Host
               && uri.AbsolutePath.Trim('/').Length == 6;
    }

    private static bool IsUrlUsingHttp(Uri uri)
    {
        return !uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) &&
               !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
    }
}