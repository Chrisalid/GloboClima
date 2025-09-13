using System.ComponentModel.DataAnnotations;

namespace GloboClima.Core.Entities;

public class FavoriteCountry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public string CountryName { get; set; } = string.Empty;
    
    [Required]
    public string CountryCode { get; set; } = string.Empty;
    
    public string Capital { get; set; } = string.Empty;
    
    public long Population { get; set; }
    
    public List<string> Languages { get; set; } = new();
    
    public List<string> Currencies { get; set; } = new();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
