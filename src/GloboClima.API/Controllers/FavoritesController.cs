using GloboClima.Core.DTOs;
using GloboClima.Core.Entities;
using GloboClima.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GloboClima.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteCityRepository _favoriteCityRepository;
    private readonly IFavoriteCountryRepository _favoriteCountryRepository;

    public FavoritesController(
        IFavoriteCityRepository favoriteCityRepository,
        IFavoriteCountryRepository favoriteCountryRepository)
    {
        _favoriteCityRepository = favoriteCityRepository;
        _favoriteCountryRepository = favoriteCountryRepository;
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new UnauthorizedAccessException("User not authenticated");
    }

    #region Cidades Favoritas

    /// <summary>
    /// Obtém todas as cidades favoritas do usuário autenticado
    /// </summary>
    /// <returns>Lista de cidades favoritas</returns>
    [HttpGet("cities")]
    public async Task<ActionResult<IEnumerable<FavoriteCity>>> GetFavoriteCities()
    {
        var userId = GetUserId();
        var favoriteCities = await _favoriteCityRepository.GetByUserIdAsync(userId);
        return Ok(favoriteCities);
    }

    /// <summary>
    /// Adiciona uma cidade aos favoritos do usuário
    /// </summary>
    /// <param name="request">Dados da cidade a ser adicionada</param>
    /// <returns>Cidade favorita criada</returns>
    [HttpPost("cities")]
    public async Task<ActionResult<FavoriteCity>> AddFavoriteCity([FromBody] AddFavoriteCityRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();

        // Verificar se a cidade já existe nos favoritos
        var exists = await _favoriteCityRepository.ExistsAsync(userId, request.CityName, request.Country);
        if (exists)
            return Conflict("Esta cidade já está nos seus favoritos");

        var favoriteCity = new FavoriteCity
        {
            UserId = userId,
            CityName = request.CityName,
            Country = request.Country,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _favoriteCityRepository.CreateAsync(favoriteCity);
        return CreatedAtAction(nameof(GetFavoriteCities), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove uma cidade dos favoritos
    /// </summary>
    /// <param name="id">ID da cidade favorita</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("cities/{id}")]
    public async Task<ActionResult> DeleteFavoriteCity(string id)
    {
        var userId = GetUserId();
        var favoriteCity = await _favoriteCityRepository.GetByIdAsync(id);

        if (favoriteCity == null)
            return NotFound("Cidade favorita não encontrada");

        if (favoriteCity.UserId != userId)
            return Forbid("Você não tem permissão para remover esta cidade");

        await _favoriteCityRepository.DeleteAsync(id);
        return NoContent();
    }

    #endregion

    #region Países Favoritos

    /// <summary>
    /// Obtém todos os países favoritos do usuário autenticado
    /// </summary>
    /// <returns>Lista de países favoritos</returns>
    [HttpGet("countries")]
    public async Task<ActionResult<IEnumerable<FavoriteCountry>>> GetFavoriteCountries()
    {
        var userId = GetUserId();
        var favoriteCountries = await _favoriteCountryRepository.GetByUserIdAsync(userId);
        return Ok(favoriteCountries);
    }

    /// <summary>
    /// Adiciona um país aos favoritos do usuário
    /// </summary>
    /// <param name="request">Dados do país a ser adicionado</param>
    /// <returns>País favorito criado</returns>
    [HttpPost("countries")]
    public async Task<ActionResult<FavoriteCountry>> AddFavoriteCountry([FromBody] AddFavoriteCountryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();

        // Verificar se o país já existe nos favoritos
        var exists = await _favoriteCountryRepository.ExistsAsync(userId, request.CountryCode);
        if (exists)
            return Conflict("Este país já está nos seus favoritos");

        var favoriteCountry = new FavoriteCountry
        {
            UserId = userId,
            CountryName = request.CountryName,
            CountryCode = request.CountryCode,
            Capital = request.Capital,
            Population = request.Population,
            Languages = request.Languages,
            Currencies = request.Currencies,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _favoriteCountryRepository.CreateAsync(favoriteCountry);
        return CreatedAtAction(nameof(GetFavoriteCountries), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove um país dos favoritos
    /// </summary>
    /// <param name="id">ID do país favorito</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("countries/{id}")]
    public async Task<ActionResult> DeleteFavoriteCountry(string id)
    {
        var userId = GetUserId();
        var favoriteCountry = await _favoriteCountryRepository.GetByIdAsync(id);

        if (favoriteCountry == null)
            return NotFound("País favorito não encontrado");

        if (favoriteCountry.UserId != userId)
            return Forbid("Você não tem permissão para remover este país");

        await _favoriteCountryRepository.DeleteAsync(id);
        return NoContent();
    }

    #endregion
}
