using GloboClima.Core.Entities;

namespace GloboClima.Core.Interfaces;

public interface IFavoriteCountryRepository
{
    Task<IEnumerable<FavoriteCountry>> GetByUserIdAsync(string userId);
    Task<FavoriteCountry?> GetByIdAsync(string id);
    Task<FavoriteCountry> CreateAsync(FavoriteCountry favoriteCountry);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string userId, string countryCode);
}
