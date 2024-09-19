using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace URLShortener.Extensions.ExceptionHandling;

public class InvalidUrlExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidUrlException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = httpContext.Response.StatusCode,
            Detail = exception.Message,
        }, cancellationToken);

        return true;
    }
}
