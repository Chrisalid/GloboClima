using System.ComponentModel.DataAnnotations;

namespace GloboClima.Core.Entities;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<FavoriteCity> FavoriteCities { get; set; } = new();
    
    public List<FavoriteCountry> FavoriteCountries { get; set; } = new();
}
