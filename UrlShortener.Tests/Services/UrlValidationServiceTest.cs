using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using URLShortener.Models.Exceptions;
using URLShortener.Services;

namespace UrlShortener.Tests.Services;

[TestSubject(typeof(UrlValidationService))]
public class UrlValidationServiceTest
{
    private readonly UrlValidationService _urlValidationService;

    public UrlValidationServiceTest()
    {
        Mock<IHttpContextAccessor> httpContextAccessor = new();
        var defaultHttpContext = new DefaultHttpContext()
        {
            Request =
            {
                Scheme = "https",
                Host = new HostString("short.url"),
            }
        };
        httpContextAccessor.Setup(x => x.HttpContext).Returns(defaultHttpContext);
        _urlValidationService = new UrlValidationService(httpContextAccessor.Object);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("not a url")]
    [InlineData("http://.com")]
    [InlineData("http:/missing-slash.com")]
    [InlineData("ftp://invalid.com")]
    [InlineData("https://invalid-regex-pattern")]
    public void ValidateUrl_InvalidUrl_ShouldThrow_InvalidUrlException(string invalidUrl)
    {
        //act & assert
        Action action = () => _urlValidationService.ValidateUrl(invalidUrl);
        action.Should().Throw<InvalidUrlException>()
            .WithMessage($"The provided URL is not valid. Url: {invalidUrl}");
    }
    
    
    [Fact]
    public void ValidateUrl_WithShortenedUrl_ShouldThrowInvalidUrlException()
    {
        // arrange
        var shortenedUrl = "https://short.url/abc123";

        // act & assert
        var action = () => _urlValidationService.ValidateUrl(shortenedUrl);
        action.Should()
            .Throw<InvalidUrlException>()
            .WithMessage($"The provided URL is already shorten. Url: {shortenedUrl}");
    }

    
    [Theory]
    [InlineData("https://www.google.com")]
    [InlineData("https://sub.domain.com/path?param=value")]
    [InlineData("http://example.com/path/to/resource#section")]
    [InlineData("https://domain.com:8080")]
    public void ValidateUrl_ValidUrl_ShouldReturnTrue(string validUrl)
    {
        //act & assert
        Action action = () => _urlValidationService.ValidateUrl(validUrl);
        action.Should().NotThrow<InvalidUrlException>();
    }
}