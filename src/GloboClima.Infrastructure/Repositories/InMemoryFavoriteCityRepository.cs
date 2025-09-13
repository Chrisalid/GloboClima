using GloboClima.Core.Entities;
using GloboClima.Core.Interfaces;

namespace GloboClima.Infrastructure.Repositories;

public class InMemoryFavoriteCityRepository : IFavoriteCityRepository
{
    private readonly List<FavoriteCity> _favoriteCities = new();
    private int _nextId = 1;

    public Task<IEnumerable<FavoriteCity>> GetByUserIdAsync(string userId)
    {
        var favorites = _favoriteCities.Where(f => f.UserId == userId).AsEnumerable();
        return Task.FromResult(favorites);
    }

    public Task<FavoriteCity?> GetByIdAsync(string id)
    {
        var favorite = _favoriteCities.FirstOrDefault(f => f.Id == id);
        return Task.FromResult(favorite);
    }

    public Task<FavoriteCity> CreateAsync(FavoriteCity favoriteCity)
    {
        favoriteCity.Id = _nextId++.ToString();
        favoriteCity.CreatedAt = DateTime.UtcNow;
        _favoriteCities.Add(favoriteCity);
        return Task.FromResult(favoriteCity);
    }

    public Task DeleteAsync(string id)
    {
        var favorite = _favoriteCities.FirstOrDefault(f => f.Id == id);
        if (favorite != null)
        {
            _favoriteCities.Remove(favorite);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string userId, string cityName, string country)
    {
        var exists = _favoriteCities.Any(f => f.UserId == userId && 
                                             f.CityName == cityName && 
                                             f.Country == country);
        return Task.FromResult(exists);
    }
}
