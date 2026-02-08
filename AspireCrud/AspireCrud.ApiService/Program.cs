using AspireCrud.ApiService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<AppDbContext>("sqldb");

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddAzureChatCompletionsClient("chat");

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/", () => "API service is running. Navigate to /weatherforecast to see sample data.");

app.MapGet("/weatherforecast", async (AppDbContext db) =>
{
    return await db.WeatherForecasts
                   .OrderBy(w => w.Date)
                   .ToListAsync();
});

app.MapPost("/weatherforecast", async (WeatherForecast forecast, AppDbContext db) =>
{
    db.WeatherForecasts.Add(forecast);
    await db.SaveChangesAsync();
    return Results.Created($"/weatherforecast/{forecast.Id}", forecast);
});

app.MapPut("/weatherforecast/{id:int}", async (int id, WeatherForecast input, AppDbContext db) =>
{
    var existing = await db.WeatherForecasts.FindAsync(id);
    if (existing is null) return Results.NotFound();

    existing.Date = input.Date;
    existing.TemperatureC = input.TemperatureC;
    existing.Summary = input.Summary;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/weatherforecast/{id:int}", async (int id, AppDbContext db) =>
{
    var existing = await db.WeatherForecasts.FindAsync(id);
    if (existing is null) return Results.NotFound();

    db.WeatherForecasts.Remove(existing);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/weatherforecast/batch", async (AppDbContext db) =>
{
    var rng = new Random();
    var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = rng.Next(-20, 55),
        Summary = summaries[rng.Next(summaries.Length)]
    }).ToArray();
    db.WeatherForecasts.AddRange(forecasts);
    await db.SaveChangesAsync();
    return Results.Created("/weatherforecast/batch", forecasts);
});

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    if (!db.WeatherForecasts.Any())
    {
        var rng = new Random();
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = rng.Next(-20, 55),
            Summary = summaries[rng.Next(summaries.Length)]
        }).ToArray();

        db.WeatherForecasts.AddRange(forecasts);
        db.SaveChanges();
    }
}

app.Run();

public class WeatherForecast
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public interface IForecastDescriber
{
    Task<string> DescribeAsync(WeatherForecast forecast);
}

public class ForecastDescriber : IForecastDescriber
{
    private readonly IChatClient _chatClient;

    public ForecastDescriber(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> DescribeAsync(WeatherForecast forecast)
    {
        var prompt = $"Describe the following weather forecast in a human-friendly way: Date: {forecast.Date}, Temp: {forecast.TemperatureC}°C, Summary: {forecast.Summary}";
        var response = await _chatClient.GetResponseAsync(prompt);
        return response.Text;
    }
}