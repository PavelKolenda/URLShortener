namespace URLShortener.Services;
public interface IUrlShorteningService
{
    Task<string> Generate();
}