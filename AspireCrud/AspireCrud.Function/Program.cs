using AspireCrud_Function;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.AddAzureChatCompletionsClient("chat").AddChatClient();

// Add service defaults & Aspire client integrations
builder.AddServiceDefaults();
builder.Services.AddScoped<IForecastDescriber, ForecastDescriber>();

// Add typed HTTP client for ApiService
builder.Services.AddHttpClient<WeatherForecastClient>(client =>
{
    client.BaseAddress = new Uri("https+http://apiservice");
});

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var app = builder.Build();
app.Run();

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