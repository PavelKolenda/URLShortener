using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;
using URLShortener.Options;
using URLShortener.Repository.Interfaces;
using URLShortener.Services;

namespace UrlShortener.Tests.Services;

[TestSubject(typeof(UrlShorteningService))]
public class UrlShorteningServiceTest
{
    private readonly UrlShorteningService _urlShorteningService;
    private readonly Mock<IUrlShortRepository> _urlRepository;
    private readonly UrlShorteningOptions _urlShorteningOptions;

    public UrlShorteningServiceTest()
    {
        _urlRepository = new Mock<IUrlShortRepository>();
        _urlShorteningOptions = new UrlShorteningOptions
        {
            NumberOfCharsInShortUrl = 6,
            Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        };
        var urlShorteningOptions = _urlShorteningOptions;
        
        var optionsMock = new Mock<IOptions<UrlShorteningOptions>>();
        optionsMock.Setup(o => o.Value).Returns(urlShorteningOptions);
        
        _urlShorteningService = new UrlShorteningService(_urlRepository.Object, optionsMock.Object);
    }

    [Fact]
    public async Task GenerateAsync_ExistedCode_ShouldCallRepositoryFewTimes()
    {
        //arrange
        var attempts = 0;
        _urlRepository.Setup(x => x.IsShortUrlUniqueAsync(It.IsAny<string>()))
            .ReturnsAsync(() => ++attempts == 3);

        //act
        var result = await _urlShorteningService.GenerateAsync();

        //assert
        result.Should().NotBeNull();
        result.Should().HaveLength(_urlShorteningOptions.NumberOfCharsInShortUrl);
        _urlRepository.Verify(x => x.IsShortUrlUniqueAsync(It.IsAny<string>()), 
            Times.Exactly(3));
    }
    
}