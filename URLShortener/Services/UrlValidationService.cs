using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services;
public class UrlValidationService : IUrlValidationService
{
    private readonly IHttpContextAccessor _httpContext;

    public UrlValidationService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public void ValidateUrl(string url)
    {

        HttpContext httpContext = _httpContext.HttpContext;

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
        {
            throw new InvalidUrlException("The provided URL is not valid.");
        }

        if (IsUrlUsingHttp(uriResult))
        {
            throw new InvalidUrlException("The URL must start with 'http' or 'https'.");
        }

        if (string.IsNullOrWhiteSpace(uriResult.Host))
        {
            throw new InvalidUrlException("The URL must have a valid host.");
        }

        if (IsUrlShorten(uriResult))
        {
            throw new InvalidUrlException("The provided URL is already shorten.");
        }

        if (!ValidateUrlWithRegex(url))
        {
            throw new InvalidUrlException("The provided URL is not valid.");
        }
    }

    private bool ValidateUrlWithRegex(string url)
    {
        var urlRegex = new Regex(
            @"^(https?):\/\/(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}" +
            @"(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5]))?" +
            @"(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$",
            RegexOptions.IgnoreCase);

        return urlRegex.IsMatch(url);
    }

    private bool IsUrlShorten(Uri uri)
    {
        HttpContext httpContext = _httpContext.HttpContext;

        if (uri.Scheme == httpContext.Request.Scheme 
            && uri.Host == httpContext.Request.Host.Host.ToString()
            && uri.AbsolutePath.Trim('/').Length == 6)
        {
            return true;
        }

        return false;
    }

    private bool IsUrlUsingHttp(Uri uri)
    {
        if (!uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) &&
            !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }
}
