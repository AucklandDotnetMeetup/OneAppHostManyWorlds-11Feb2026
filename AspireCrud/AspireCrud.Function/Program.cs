using AspireCrud_Function;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Add service defaults & Aspire client integrations
builder.AddServiceDefaults();

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

