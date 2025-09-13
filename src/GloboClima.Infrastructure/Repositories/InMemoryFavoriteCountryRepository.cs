using GloboClima.Core.Entities;
using GloboClima.Core.Interfaces;

namespace GloboClima.Infrastructure.Repositories;

public class InMemoryFavoriteCountryRepository : IFavoriteCountryRepository
{
    private readonly List<FavoriteCountry> _favoriteCountries = new();
    private int _nextId = 1;

    public Task<IEnumerable<FavoriteCountry>> GetByUserIdAsync(string userId)
    {
        var favorites = _favoriteCountries.Where(f => f.UserId == userId).AsEnumerable();
        return Task.FromResult(favorites);
    }

    public Task<FavoriteCountry?> GetByIdAsync(string id)
    {
        var favorite = _favoriteCountries.FirstOrDefault(f => f.Id == id);
        return Task.FromResult(favorite);
    }

    public Task<FavoriteCountry> CreateAsync(FavoriteCountry favoriteCountry)
    {
        favoriteCountry.Id = _nextId++.ToString();
        favoriteCountry.CreatedAt = DateTime.UtcNow;
        _favoriteCountries.Add(favoriteCountry);
        return Task.FromResult(favoriteCountry);
    }

    public Task DeleteAsync(string id)
    {
        var favorite = _favoriteCountries.FirstOrDefault(f => f.Id == id);
        if (favorite != null)
        {
            _favoriteCountries.Remove(favorite);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string userId, string countryCode)
    {
        var exists = _favoriteCountries.Any(f => f.UserId == userId && f.CountryCode == countryCode);
        return Task.FromResult(exists);
    }
}
