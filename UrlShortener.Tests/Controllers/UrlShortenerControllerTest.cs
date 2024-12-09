using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using URLShortener.Controllers;
using URLShortener.Models;
using URLShortener.Services.Interfaces;

namespace UrlShortener.Tests.Controllers;

[TestSubject(typeof(UrlShortenerController))]
public class UrlShortenerControllerTest
{
    [Fact]
    public async Task ShortenUrl_Returns_ShortenedUrl()
    {
        //arrange
        var mockService = new Mock<IUrlShortenerService>();
        var mockHttpContext = new DefaultHttpContext();
        
        var request = new UrlShortenRequest { LongUrl = "https://example.com" };
        var shortenedUrl = new ShortenedUrl
        {
            LongUrl = request.LongUrl,
            ShortUrl = "localhost:1234/qwerty1",
            CreatedAt = DateTime.Now
        };
        
        mockService.Setup(service => service.CreateShortUrlAsync(request, mockHttpContext))
            .ReturnsAsync(shortenedUrl);
        
        var controller = new UrlShortenerController(mockService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            }
        };
        //act
        var result = await controller.Shorten(request);
        //assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ShortenedUrl>(okResult.Value);
        
        Assert.Equal(shortenedUrl.LongUrl, returnValue.LongUrl);
        Assert.Equal(shortenedUrl.ShortUrl, returnValue.ShortUrl);
        Assert.Equal(shortenedUrl.CreatedAt, returnValue.CreatedAt);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        mockService.Verify(service => service.CreateShortUrlAsync(request, mockHttpContext), Times.Once);
    }

    [Fact]
    public async Task RedirectToLongUrl_Redirects_To_LongUrl()
    {
        var mockService = new Mock<IUrlShortenerService>();
        const string shortUrlCode = "qwerty1";
        const string longUrlResponse = "https://example.com";
        
        mockService.Setup(service => service.GetLongUrlAsync(shortUrlCode))
            .ReturnsAsync(longUrlResponse);

        var controller = new UrlShortenerController(mockService.Object);
        
        var result = await controller.RedirectToLongUrl(shortUrlCode);
        
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal(longUrlResponse, redirectResult.Url);
        mockService.Verify(service => service.GetLongUrlAsync(shortUrlCode), Times.Once);
    }
}