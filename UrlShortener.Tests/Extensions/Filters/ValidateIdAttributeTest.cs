using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using URLShortener.Extensions.Filters;

namespace UrlShortener.Tests.Extensions.Filters;

[TestSubject(typeof(ValidateIdAttribute))]
public class ValidateIdAttributeTest
{

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(int.MaxValue + 1L)]
    public void OnActionExecuting_InvalidId_ReturnsBadRequest(object invalidId)
    {
        // arrange
        var filter = new ValidateIdAttribute();
        var actionExecutingContext = GetActionExecutingContext(invalidId);

        // Act
        filter.OnActionExecuting(actionExecutingContext);

        // Assert
        actionExecutingContext.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(9999999)]
    public void OnActionExecuting_ValidId_AllowsExecution(object validId)
    {
        //arrange
        var filter = new ValidateIdAttribute();
        var actionExecutingContext = GetActionExecutingContext(validId);
        
        //act
        filter.OnActionExecuting(actionExecutingContext);
        
        //assert
        actionExecutingContext.Result.Should().BeNull();
    }

    private static ActionExecutingContext GetActionExecutingContext(object id)
    {
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());
        
        var actionArgs = new Dictionary<string, object> { ["id"] = id };
        
        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            actionArgs!,
            Mock.Of<Controller>());
        
        return actionExecutingContext;
    }
}