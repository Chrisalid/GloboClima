using GloboClima.Core.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GloboClima.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? "http://localhost:7001/api/");
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuthorizationHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    // Auth Methods
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("auth/login", content);
        
        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(responseContent, _jsonOptions);
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("auth/register", content);
        
        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(responseContent, _jsonOptions);
    }

    // Weather Methods
    public async Task<WeatherResponse?> GetWeatherAsync(string cityName)
    {
        try
        {
            var response = await _httpClient.GetAsync($"weather/{cityName}");
            
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherResponse>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<WeatherResponse?> GetWeatherByCoordinatesAsync(double latitude, double longitude)
    {
        try
        {
            var response = await _httpClient.GetAsync($"weather/coordinates?latitude={latitude}&longitude={longitude}");
            
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherResponse>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    // Country Methods
    public async Task<List<CountryResponse>> GetCountriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("countries");
            
            if (!response.IsSuccessStatusCode)
                return new List<CountryResponse>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CountryResponse>>(content, _jsonOptions) ?? new List<CountryResponse>();
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
            var response = await _httpClient.GetAsync($"countries/name/{countryName}");
            
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CountryResponse>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    // Favorite Methods
    public async Task<List<FavoriteLocationResponse>> GetFavoritesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("favorites");
            
            if (!response.IsSuccessStatusCode)
                return new List<FavoriteLocationResponse>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<FavoriteLocationResponse>>(content, _jsonOptions) ?? new List<FavoriteLocationResponse>();
        }
        catch
        {
            return new List<FavoriteLocationResponse>();
        }
    }

    public async Task<bool> AddFavoriteAsync(AddFavoriteLocationRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("favorites", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveFavoriteAsync(int favoriteId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"favorites/{favoriteId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<GloboClima.Core.Entities.FavoriteCity>> GetFavoriteCitiesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("favorites/cities");
            
            if (!response.IsSuccessStatusCode)
                return new List<GloboClima.Core.Entities.FavoriteCity>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<GloboClima.Core.Entities.FavoriteCity>>(content, _jsonOptions) ?? new List<GloboClima.Core.Entities.FavoriteCity>();
        }
        catch
        {
            return new List<GloboClima.Core.Entities.FavoriteCity>();
        }
    }

    public async Task<bool> AddFavoriteCityAsync(AddFavoriteCityRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("favorites/cities", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteFavoriteCityAsync(string id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"favorites/cities/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<GloboClima.Core.Entities.FavoriteCountry>> GetFavoriteCountriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("favorites/countries");
            
            if (!response.IsSuccessStatusCode)
                return new List<GloboClima.Core.Entities.FavoriteCountry>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<GloboClima.Core.Entities.FavoriteCountry>>(content, _jsonOptions) ?? new List<GloboClima.Core.Entities.FavoriteCountry>();
        }
        catch
        {
            return new List<GloboClima.Core.Entities.FavoriteCountry>();
        }
    }

    public async Task<bool> AddFavoriteCountryAsync(AddFavoriteCountryRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("favorites/countries", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteFavoriteCountryAsync(string id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"favorites/countries/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
