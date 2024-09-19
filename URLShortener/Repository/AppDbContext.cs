using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Repository;
public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var serverVersion = new MySqlServerVersion(new Version(_configuration["MySqlOptions:ServerVersion"]));
        options.UseMySql(_configuration.GetConnectionString("MySql"), serverVersion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.HasIndex(p => p.ShortUrl).IsUnique();
        });
    }
}

