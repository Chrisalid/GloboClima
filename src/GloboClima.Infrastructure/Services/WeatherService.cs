using GloboClima.Core.DTOs;
using GloboClima.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GloboClima.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5";

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = !string.IsNullOrWhiteSpace(configuration["OpenWeatherMap:ApiKey"]) ? configuration["OpenWeatherMap:ApiKey"] :
            throw new ArgumentNullException("OpenWeatherMap API Key is required");
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string cityName)
    {
        try
        {
            var url = $"{_baseUrl}/weather?q={cityName}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                return null;

            var jsonContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherResponse>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }

    public async Task<WeatherResponse?> GetWeatherAsync(double latitude, double longitude)
    {
        try
        {
            var url = $"{_baseUrl}/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                return null;

            var jsonContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherResponse>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }
}
