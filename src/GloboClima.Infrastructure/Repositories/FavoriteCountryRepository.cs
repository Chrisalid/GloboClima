using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClima.Core.Entities;
using GloboClima.Core.Interfaces;
using System.Text.Json;

namespace GloboClima.Infrastructure.Repositories;

public class FavoriteCountryRepository : IFavoriteCountryRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly Table _table;
    private const string TableName = "GloboClima-FavoriteCountries";

    public FavoriteCountryRepository(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
        _table = Table.LoadTable(_dynamoDbClient, TableName);
    }

    public async Task<IEnumerable<FavoriteCountry>> GetByUserIdAsync(string userId)
    {
        try
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("UserId", ScanOperator.Equal, userId);
            
            var search = _table.Scan(scanFilter);
            var documents = await search.GetNextSetAsync();
            
            return documents.Select(doc => JsonSerializer.Deserialize<FavoriteCountry>(doc.ToJson())!);
        }
        catch
        {
            return Enumerable.Empty<FavoriteCountry>();
        }
    }

    public async Task<FavoriteCountry?> GetByIdAsync(string id)
    {
        try
        {
            var document = await _table.GetItemAsync(id);
            return document != null ? JsonSerializer.Deserialize<FavoriteCountry>(document.ToJson()) : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<FavoriteCountry> CreateAsync(FavoriteCountry favoriteCountry)
    {
        var document = Document.FromJson(JsonSerializer.Serialize(favoriteCountry));
        await _table.PutItemAsync(document);
        return favoriteCountry;
    }

    public async Task DeleteAsync(string id)
    {
        await _table.DeleteItemAsync(id);
    }

    public async Task<bool> ExistsAsync(string userId, string countryCode)
    {
        try
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("UserId", ScanOperator.Equal, userId);
            scanFilter.AddCondition("CountryCode", ScanOperator.Equal, countryCode);
            
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
