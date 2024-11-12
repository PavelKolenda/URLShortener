using URLShortener.Extensions.ExceptionHandling;

namespace URLShortener.Extensions;

public static class ServiceExtensions
{
    public static void AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<InvalidUrlExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}