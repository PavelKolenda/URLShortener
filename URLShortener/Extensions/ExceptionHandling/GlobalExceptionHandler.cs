using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace URLShortener.Extensions.ExceptionHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        httpContext.Response.WriteAsJsonAsync(new ProblemDetails { Status = httpContext.Response.StatusCode },
            cancellationToken);

        logger.LogError(exception, exception.Message);

        return ValueTask.FromResult(true);
    }
}