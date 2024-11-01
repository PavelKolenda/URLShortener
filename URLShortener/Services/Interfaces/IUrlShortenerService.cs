﻿using URLShortener.Models;

namespace URLShortener.Services.Interfaces;

public interface IUrlShortenerService
{
    Task<ShortenedUrl> CreateShortUrlAsync(UrlShortenRequest request, HttpContext httpContext);
    Task<string> GetLongUrlAsync(string shortUrl);
}