using GloboClima.Core.Entities;

namespace GloboClima.Core.Interfaces;

public interface IFavoriteCityRepository
{
    Task<IEnumerable<FavoriteCity>> GetByUserIdAsync(string userId);
    Task<FavoriteCity?> GetByIdAsync(string id);
    Task<FavoriteCity> CreateAsync(FavoriteCity favoriteCity);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string userId, string cityName, string country);
}
