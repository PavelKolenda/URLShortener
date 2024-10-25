using Microsoft.EntityFrameworkCore;
using URLShortener.Repository;

namespace URLShortener.Extensions;
public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}