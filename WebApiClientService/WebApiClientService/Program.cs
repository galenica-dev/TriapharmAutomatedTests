using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWindowsService();

// Add controllers and configure JSON serialization to use kebab-case
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.KebabCaseLower;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();

// Explicitly map HomeController at `/` to avoid ASP0014 warning
app.MapGet("/", async (HttpContext context) =>
{
    var result = new
    {
        appName = "WebApiClientService",
        getVersionApp = "/api/version",
        getAhv = "/api/gen-data/ahv",
        getInsuranceCardNumber = "/api/gen-data/insurance-card-number",
        postRemoteCmd = "/remote/cmd/execute",
        timestamp = DateTime.UtcNow
    };

    await context.Response.WriteAsJsonAsync(result, new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
        DictionaryKeyPolicy = JsonNamingPolicy.KebabCaseLower
    });
});

// Still allows other API controllers
app.MapControllers();

app.Run();

