using System;
using AspireCrud_Function;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AspireCrud.Function;

public class WeatherSummaryEnricher
{
    private readonly ILogger _logger;
    private readonly WeatherForecastClient _weatherClient;
    private readonly IForecastDescriber _describer;

    public WeatherSummaryEnricher(ILoggerFactory loggerFactory, WeatherForecastClient weatherClient, IForecastDescriber describer)
    {
        _logger = loggerFactory.CreateLogger<WeatherSummaryEnricher>();
        _weatherClient = weatherClient;
        _describer = describer;
    }

    [Function("WeatherSummaryEnricher")]
    public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }

        try
        {
            var forecasts = await _weatherClient.GetAllForecastsAsync();
            _logger.LogInformation("Successfully retrieved {count} weather forecasts", forecasts?.Count ?? 0);
            
            foreach (var forecast in forecasts ?? [])
            {
                _logger.LogInformation("Forecast for {date}: {summary} with {tempC}°C", forecast.Date, forecast.Summary, forecast.TemperatureC);
                
                // Determine correct summary based on temperature
                var correctSummary = forecast.TemperatureC switch
                {
                    < 0 => "Freezing",
                    >= 0 and <= 5 => "Bracing",
                    >= 6 and <= 10 => "Chilly",
                    >= 11 and <= 15 => "Cool",
                    >= 16 and <= 20 => "Mild",
                    >= 21 and <= 25 => "Warm",
                    >= 26 and <= 30 => "Balmy",
                    >= 31 and <= 35 => "Hot",
                    >= 36 and <= 45 => "Sweltering",
                    _ => "Scorching"
                };

                // Update if summary is incorrect
                if (forecast.Summary != correctSummary || string.IsNullOrEmpty(forecast.Description))
                {
                    _logger.LogInformation("Updating forecast {id}: '{oldSummary}' -> '{newSummary}'", 
                        forecast.Id, forecast.Summary, correctSummary);
                    
                    forecast.Summary = correctSummary;
                    forecast.Description = await _describer.DescribeAsync(forecast);
                    await _weatherClient.UpdateForecastAsync(forecast.Id, forecast);
                }
            }
            
            _logger.LogInformation("Weather summary enrichment completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling ApiService");
        }
    }
}