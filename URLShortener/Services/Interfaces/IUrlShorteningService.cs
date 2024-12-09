namespace URLShortener.Services.Interfaces;

public interface IUrlShorteningService
{
    Task<string> GenerateAsync();
}