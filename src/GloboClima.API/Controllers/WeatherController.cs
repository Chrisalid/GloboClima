using GloboClima.Core.DTOs;
using GloboClima.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    /// <summary>
    /// Obtém informações climáticas de uma cidade
    /// </summary>
    /// <param name="cityName">Nome da cidade</param>
    /// <returns>Dados climáticos da cidade</returns>
    [HttpGet("{cityName}")]
    public async Task<ActionResult<WeatherResponse>> GetWeather(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return BadRequest("Nome da cidade é obrigatório");

        var weather = await _weatherService.GetWeatherAsync(cityName);
        
        if (weather == null)
            return NotFound($"Dados climáticos não encontrados para a cidade: {cityName}");

        return Ok(weather);
    }

    /// <summary>
    /// Obtém informações climáticas por coordenadas geográficas
    /// </summary>
    /// <param name="latitude">Latitude</param>
    /// <param name="longitude">Longitude</param>
    /// <returns>Dados climáticos das coordenadas</returns>
    [HttpGet("coordinates")]
    public async Task<ActionResult<WeatherResponse>> GetWeatherByCoordinates(
        [FromQuery] double latitude,
        [FromQuery] double longitude)
    {
        var weather = await _weatherService.GetWeatherAsync(latitude, longitude);
        
        if (weather == null)
            return NotFound($"Dados climáticos não encontrados para as coordenadas: {latitude}, {longitude}");

        return Ok(weather);
    }
}
