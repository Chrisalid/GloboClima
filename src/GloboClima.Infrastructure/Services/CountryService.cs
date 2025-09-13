using GloboClima.Core.DTOs;
using GloboClima.Core.Interfaces;
using System.Text.Json;

namespace GloboClima.Infrastructure.Services;

public class CountryService : ICountryService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://restcountries.com/v3.1";

    public CountryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CountryResponse>> GetCountriesAsync()
    {
        try
        {
            var url = $"{_baseUrl}/all?fields=name,capital,region,subregion,population,area,languages,flag,currencies";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                return new List<CountryResponse>();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<CountryResponse>>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return countries ?? new List<CountryResponse>();
        }
        catch
        {
            return new List<CountryResponse>();
        }
    }

    public async Task<CountryResponse?> GetCountryByNameAsync(string countryName)
    {
        try
        {
            var url = $"{_baseUrl}/name/{countryName}";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                return null;

            var jsonContent = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<CountryResponse>>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return countries?.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    public async Task<CountryResponse?> GetCountryByCodeAsync(string countryCode)
    {
        try
        {
            var url = $"{_baseUrl}/alpha/{countryCode}";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                return null;

            var jsonContent = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<CountryResponse>>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return countries?.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }
}
