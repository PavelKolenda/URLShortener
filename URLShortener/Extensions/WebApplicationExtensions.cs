using Microsoft.EntityFrameworkCore;
using URLShortener.Repository;

namespace URLShortener.Extensions;
public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}