using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Repository;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; init; }

    private const string MySqlServerVersion = "8.0.39";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        MySqlServerVersion serverVersion =
            new(new Version(configuration["MySqlOptions:ServerVersion"] ?? MySqlServerVersion));
        optionsBuilder.UseMySql(configuration.GetConnectionString("MySqlDocker"), serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.HasIndex(p => p.ShortUrl).IsUnique();
        });
    }
}