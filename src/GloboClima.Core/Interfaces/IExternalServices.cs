using GloboClima.Core.DTOs;
using GloboClima.Core.Entities;

namespace GloboClima.Core.Interfaces;

public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherAsync(string cityName);
    Task<WeatherResponse?> GetWeatherAsync(double latitude, double longitude);
}

public interface ICountryService
{
    Task<List<CountryResponse>> GetCountriesAsync();
    Task<CountryResponse?> GetCountryByNameAsync(string countryName);
    Task<CountryResponse?> GetCountryByCodeAsync(string countryCode);
}
