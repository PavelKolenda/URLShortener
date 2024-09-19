using URLShortener.Services.Interfaces;

namespace URLShortener.Services;
public class UrlValidationService : IUrlValidationService
{
    public void ValidateUrl(string url)
    {
        if (url.Contains("localhost:"))
        {
            throw new InvalidUrlException("Long URL shouldn't contain 'localhost'");
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
        {
            throw new InvalidUrlException("The provided URL is not valid.");
        }

        if (string.IsNullOrWhiteSpace(uriResult.Host))
        {
            throw new InvalidUrlException("The URL should have a valid host.");
        }
    }
}
