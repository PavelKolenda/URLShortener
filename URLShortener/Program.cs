using URLShortener.Extensions;
using URLShortener.Options;
using URLShortener.Repository;
using URLShortener.Services;
using URLShortener.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandling();

builder.Services.Configure<UrlShorteningOptions>(builder.Configuration.GetSection("UrlShorteningOptions"));

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IUrlShortRepository, UrlShortRepository>();
builder.Services.AddScoped<IUrlManagementService, UrlManagementService>();
builder.Services.AddScoped<IUrlValidationService, UrlValidationService>();
builder.Services.AddScoped<IUrlShorteningService, UrlShorteningService>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(_ => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
