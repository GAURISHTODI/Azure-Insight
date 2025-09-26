using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configure Local Services ---
builder.Services.AddSingleton<ShorteningService>();
// Use an in-memory dictionary to store URL mappings
builder.Services.AddSingleton<ConcurrentDictionary<string, string>>(new ConcurrentDictionary<string, string>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. Configure HTTP Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// --- 3. Define API Endpoints ---
app.MapPost("/shorten", async (
    [FromBody] ShortenRequest request,
    [FromServices] ShorteningService shorteningService,
    [FromServices] ConcurrentDictionary<string, string> urlMappings) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Invalid URL provided.");
    }
    var shortCode = await shorteningService.GenerateUniqueShortCode();
    urlMappings.TryAdd(shortCode, request.Url);
    // Use the BaseUrl from appsettings.Development.json
    var resultUrl = $"{app.Configuration["BaseUrl"]}/{shortCode}";
    return Results.Ok(new { ShortUrl = resultUrl });
});

app.MapGet("/{shortCode}", (
    string shortCode,
    [FromServices] ConcurrentDictionary<string, string> urlMappings) =>
{
    if (!urlMappings.TryGetValue(shortCode, out var originalUrl))
    {
        return Results.NotFound();
    }
    return Results.Redirect(originalUrl, permanent: false);
});

app.Run();

public record ShortenRequest(string Url);