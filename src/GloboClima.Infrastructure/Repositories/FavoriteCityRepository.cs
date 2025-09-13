using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClima.Core.Entities;
using GloboClima.Core.Interfaces;
using System.Text.Json;

namespace GloboClima.Infrastructure.Repositories;

public class FavoriteCityRepository : IFavoriteCityRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly Table _table;
    private const string TableName = "GloboClima-FavoriteCities";

    public FavoriteCityRepository(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
        _table = Table.LoadTable(_dynamoDbClient, TableName);
    }

    public async Task<IEnumerable<FavoriteCity>> GetByUserIdAsync(string userId)
    {
        try
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("UserId", ScanOperator.Equal, userId);
            
            var search = _table.Scan(scanFilter);
            var documents = await search.GetNextSetAsync();
            
            return documents.Select(doc => JsonSerializer.Deserialize<FavoriteCity>(doc.ToJson())!);
        }
        catch
        {
            return Enumerable.Empty<FavoriteCity>();
        }
    }

    public async Task<FavoriteCity?> GetByIdAsync(string id)
    {
        try
        {
            var document = await _table.GetItemAsync(id);
            return document != null ? JsonSerializer.Deserialize<FavoriteCity>(document.ToJson()) : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<FavoriteCity> CreateAsync(FavoriteCity favoriteCity)
    {
        var document = Document.FromJson(JsonSerializer.Serialize(favoriteCity));
        await _table.PutItemAsync(document);
        return favoriteCity;
    }

    public async Task DeleteAsync(string id)
    {
        await _table.DeleteItemAsync(id);
    }

    public async Task<bool> ExistsAsync(string userId, string cityName, string country)
    {
        try
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("UserId", ScanOperator.Equal, userId);
            scanFilter.AddCondition("CityName", ScanOperator.Equal, cityName);
            scanFilter.AddCondition("Country", ScanOperator.Equal, country);
            
            var search = _table.Scan(scanFilter);
            var documents = await search.GetNextSetAsync();
            
            return documents.Any();
        }
        catch
        {
            return false;
        }
    }
}
