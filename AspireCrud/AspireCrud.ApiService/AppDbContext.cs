using System;
using Microsoft.EntityFrameworkCore;

namespace AspireCrud.ApiService;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();
}
