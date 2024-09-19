using Microsoft.Extensions.Configuration;
using URLShortener.Extensions;
using URLShortener.Options;
using URLShortener.Repository;
using URLShortener.Services;
using URLShortener.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandling();

builder.Services.Configure<UrlShorteningOptions>(builder.Configuration.GetSection("UrlShorteningOptions"));

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IUrlShortRepository, UrlShortRepository>();
builder.Services.AddScoped<IUrlManagementService, UrlManagementService>();
builder.Services.AddScoped<IUrlValidationService, UrlValidationService>();
builder.Services.AddScoped<IUrlShorteningService, UrlShorteningService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
