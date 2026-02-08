using System;
using System.Net.Http.Json;

namespace AspireCrud_Function;

public class WeatherForecastClient
{
    private readonly HttpClient _httpClient;

    public WeatherForecastClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<WeatherForecast>?> GetAllForecastsAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<List<WeatherForecast>>("/weatherforecast", cancellationToken);
    }

    public async Task<WeatherForecast?> CreateForecastAsync(WeatherForecast forecast, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/weatherforecast", forecast, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WeatherForecast>(cancellationToken: cancellationToken);
    }

    public async Task UpdateForecastAsync(int id, WeatherForecast forecast, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"/weatherforecast/{id}", forecast, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteForecastAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/weatherforecast/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<WeatherForecast>?> CreateBatchForecastsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync("/weatherforecast/batch", null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<WeatherForecast>>(cancellationToken: cancellationToken);
    }
}

public class WeatherForecast
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

