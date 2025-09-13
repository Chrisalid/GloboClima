using System.ComponentModel.DataAnnotations;

namespace GloboClima.Core.DTOs;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class AddFavoriteLocationRequest
{
    [Required]
    public string CityName { get; set; } = string.Empty;
    
    [Required]
    public string CountryName { get; set; } = string.Empty;
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}

public class FavoriteLocationResponse
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime AddedAt { get; set; }
}

public class AddFavoriteCityRequest
{
    [Required]
    public string CityName { get; set; } = string.Empty;
    
    [Required]
    public string Country { get; set; } = string.Empty;
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}

public class AddFavoriteCountryRequest
{
    [Required]
    public string CountryName { get; set; } = string.Empty;
    
    [Required]
    public string CountryCode { get; set; } = string.Empty;
    
    public string Capital { get; set; } = string.Empty;
    
    public long Population { get; set; }
    
    public List<string> Languages { get; set; } = new();
    
    public List<string> Currencies { get; set; } = new();
}
