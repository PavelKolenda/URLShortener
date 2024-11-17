using AutoFixture;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using URLShortener.Controllers;
using URLShortener.Extensions.Pagination;
using URLShortener.Models;
using URLShortener.Services.Interfaces;
using FluentAssertions;
using URLShortener.DTO;

namespace UrlShortener.Tests.Controllers;

[TestSubject(typeof(UrlManagementController))]
public class UrlManagementControllerTest
{

    [Fact]
    public async Task Get_Urls_Returns_Urls_PagedList()
    {
        var mockService = new Mock<IUrlService>();
        var fixture = new Fixture();
        
        var pagedParams = new PagedParams()
        {
            Page = 1,
            PageSize = 10,
        };
        
        var urls = fixture.CreateMany<ShortenedUrl>(pagedParams.PageSize).ToList();
        
        var pagedList = PagedListExtensions
            .CreatePagedListFromList(urls, pagedParams.Page, pagedParams.PageSize);
        
        mockService.Setup(service => service.GetPagedAsync(pagedParams))
            .ReturnsAsync(pagedList);
        
        var controller = new UrlManagementController(mockService.Object);

        var result = await controller.Get(pagedParams);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPagedList = Assert.IsType<PagedList<ShortenedUrl>>(okResult.Value);
        
        returnedPagedList.TotalCount.Should().Be(pagedList.TotalCount);
        returnedPagedList.PageSize.Should().Be(pagedList.PageSize);
        returnedPagedList.Page.Should().Be(pagedList.Page);
        returnedPagedList.TotalCount.Should().Be(pagedList.TotalCount);
        mockService.Verify(service => service.GetPagedAsync(pagedParams), Times.Once);
    }

    [Fact]
    public async Task Delete_Url_Returns_NoContent()
    {
        var mockService = new Mock<IUrlService>();
        const int id = 123;
        
        mockService.Setup(service => service.DeleteAsync(id))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        var controller = new UrlManagementController(mockService.Object);
        
        var result = await controller.Delete(id);
        
        result.Should().BeOfType<NoContentResult>();
        mockService.Verify(service => service.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Update_Url_Returns_NoContent()
    {
        var mockService = new Mock<IUrlService>();
        const int id = 123;
        var shortenedUrlUpdateDto = new ShortenedUrlUpdateDto() { LongUrl = "https://example.com" };
        
        mockService.Setup(service => service.UpdateAsync(id, shortenedUrlUpdateDto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var controller = new UrlManagementController(mockService.Object);   
        var result = await controller.Update(id, shortenedUrlUpdateDto);
        
        result.Should().BeOfType<NoContentResult>();
        mockService.Verify(service => service.UpdateAsync(id, shortenedUrlUpdateDto), Times.Once);
    }
}