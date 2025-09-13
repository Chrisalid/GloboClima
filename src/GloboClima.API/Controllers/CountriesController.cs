using GloboClima.Core.DTOs;
using GloboClima.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountriesController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    /// <summary>
    /// Obtém lista de todos os países
    /// </summary>
    /// <returns>Lista de países</returns>
    [HttpGet]
    public async Task<ActionResult<List<CountryResponse>>> GetCountries()
    {
        var countries = await _countryService.GetCountriesAsync();
        return Ok(countries);
    }

    /// <summary>
    /// Obtém informações de um país específico pelo nome
    /// </summary>
    /// <param name="countryName">Nome do país</param>
    /// <returns>Dados do país</returns>
    [HttpGet("name/{countryName}")]
    public async Task<ActionResult<CountryResponse>> GetCountryByName(string countryName)
    {
        if (string.IsNullOrWhiteSpace(countryName))
            return BadRequest("Nome do país é obrigatório");

        var country = await _countryService.GetCountryByNameAsync(countryName);
        
        if (country == null)
            return NotFound($"País não encontrado: {countryName}");

        return Ok(country);
    }

    /// <summary>
    /// Obtém informações de um país específico pelo código
    /// </summary>
    /// <param name="countryCode">Código do país (ISO 3166-1 alpha-2 ou alpha-3)</param>
    /// <returns>Dados do país</returns>
    [HttpGet("code/{countryCode}")]
    public async Task<ActionResult<CountryResponse>> GetCountryByCode(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            return BadRequest("Código do país é obrigatório");

        var country = await _countryService.GetCountryByCodeAsync(countryCode);
        
        if (country == null)
            return NotFound($"País não encontrado com o código: {countryCode}");

        return Ok(country);
    }
}
