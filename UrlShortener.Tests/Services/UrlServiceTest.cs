using AutoFixture;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using URLShortener.DTO;
using URLShortener.Extensions.Pagination;
using URLShortener.Models;
using URLShortener.Models.Exceptions;
using URLShortener.Repository.Interfaces;
using URLShortener.Services;
using URLShortener.Services.Interfaces;

namespace UrlShortener.Tests.Services;

[TestSubject(typeof(UrlService))]
public class UrlServiceTest
{
    private readonly Mock<IUrlValidationService> _mockValidationService;
    private readonly Mock<IUrlShortRepository> _mockRepository;
    private readonly UrlService _urlService;

    public UrlServiceTest()
    {
        _mockValidationService = new Mock<IUrlValidationService>();
        _mockRepository = new Mock<IUrlShortRepository>();
        _urlService = new UrlService(_mockRepository.Object, _mockValidationService.Object);
    }

    [Fact]
    public async Task GetPagedAsync_Returns_ShortenedUrl_PagedList()
    {
        var fixture = new Fixture();
        var pagedParams = new PagedParams
        {
            Page = 1,
            PageSize = 10,
        };
        var urls = fixture.CreateMany<ShortenedUrl>(pagedParams.PageSize).ToList();
        var pagedList = PagedListExtensions
            .CreatePagedListFromList(urls, pagedParams.Page, pagedParams.PageSize);
        
        _mockRepository.Setup(repo => repo.GetPagedAsync(pagedParams))
            .ReturnsAsync(pagedList);
        
        var result = await _urlService.GetPagedAsync(pagedParams);

        result.Should().NotBeNull();
        result.Page.Should().Be(pagedParams.Page);
        result.PageSize.Should().Be(pagedParams.PageSize);
        result.TotalCount.Should().Be(pagedParams.PageSize);
        result.Should().BeOfType<PagedList<ShortenedUrl>>();
        _mockRepository.Verify(service => service.GetPagedAsync(pagedParams), Times.Once);
    }

    [Fact]
    public async Task GetPagedAsync_Returns_EmptyPagedList()
    {
        //arrage
        var pagedParams = new PagedParams
        {
            Page = 1,
            PageSize = 10,
        };
        var pagedList = PagedListExtensions
            .CreatePagedListFromList(new List<ShortenedUrl>(), pagedParams.Page, pagedParams.PageSize);
        
        _mockRepository.Setup(repo => repo.GetPagedAsync(pagedParams)).ReturnsAsync(pagedList);
        
        var result = await _urlService.GetPagedAsync(pagedParams);
        
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedList<ShortenedUrl>>();
        result.TotalCount.Should().Be(0);
        _mockRepository.Verify(service => service.GetPagedAsync(pagedParams), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ValidId()
    {
        //arrange
        var id = 123;
        _mockRepository.Setup(repo => repo.DeleteAsync(id))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        //act
        await _urlService.DeleteAsync(id);
        
        //assert
        _mockRepository.Verify(repo => repo.DeleteAsync(id), Times.Once);
    }
    
    [Fact]
    public void DeleteAsync_NegativeId_ThrowsArgumentException()
    {
        //arrange
        var invalidId = -123;

        //act
        Func<Task> action = async () => await _urlService.DeleteAsync(invalidId);

        //assert
        action.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"Invalid Id:{invalidId}");
    }

    [Fact]
    public async Task UpdateAsync_ValidUrlAndId_UpdatesShortenedUrl()
    {
        //arrange
        var id = 123;
        var updateDto = new ShortenedUrlUpdateDto() { LongUrl = "https://example.com" };
        var existingUrl = new ShortenedUrl { Id = id, LongUrl = "https://www.old-url.com", ShortUrl = "url" };
        
        _mockValidationService.Setup(service => service.ValidateUrl(updateDto.LongUrl)).Verifiable();
        _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingUrl);
        
        //act
        await _urlService.UpdateAsync(id, updateDto);
        
        //assert
        existingUrl.LongUrl.Should().Be(updateDto.LongUrl);
        _mockValidationService.Verify(service => service.ValidateUrl(updateDto.LongUrl), Times.Once);
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _mockRepository.Verify(repo => repo.UpdateAsync(
                It.Is<ShortenedUrl>(s => s.Id == id && s.LongUrl == updateDto.LongUrl)),
            Times.Once);
    }
    

    [Fact]
    public async Task UpdateAsync_InvalidUrl_ThrowsValidationException()
    {
        //arrange
        var id = 123;
        ShortenedUrlUpdateDto urlUpdateDto = new() { LongUrl = "https://examplecom"};
        _mockValidationService.Setup(service => service.ValidateUrl(urlUpdateDto.LongUrl))
            .Throws(new InvalidUrlException("Invalid Url"));
        
        //act
        Func<Task> action = async() => await _urlService.UpdateAsync(id, urlUpdateDto);
        
        //assert
        await action.Should().ThrowAsync<InvalidUrlException>().WithMessage("Invalid Url");
        _mockValidationService.Verify(service => service.ValidateUrl(urlUpdateDto.LongUrl), Times.Once);
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Never);
    }

    [Fact]
    public void UpdateAsync_NotExistingUrl_ThrowsNotFoundException()
    {
        //arrange
        var id = int.MaxValue;
        ShortenedUrlUpdateDto urlUpdateDto = new() { LongUrl = "https://example.com"};
        _mockRepository.Setup(repo => repo.GetByIdAsync(id)).Throws(new NotFoundException("Url not found"));
        
        //act
        Func<Task> action = async () => await _urlService.UpdateAsync(id, urlUpdateDto);

        //assert
        action.Should().ThrowAsync<NotFoundException>().WithMessage("Url not found");
        _mockValidationService.Verify(service => service.ValidateUrl(urlUpdateDto.LongUrl), Times.Once);
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ShortenedUrl>()), Times.Never);
    }
}