using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AspireCrud.Function;

public class WeatherSummaryEnricher
{
    private readonly ILogger _logger;

    public WeatherSummaryEnricher(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<WeatherSummaryEnricher>();
    }

    [Function("WeatherSummaryEnricher")]
    public void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}