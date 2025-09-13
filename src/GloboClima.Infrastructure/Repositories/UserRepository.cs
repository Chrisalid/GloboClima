using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClima.Core.Entities;
using GloboClima.Core.Interfaces;
using System.Text.Json;

namespace GloboClima.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private readonly Table _table;
    private const string TableName = "GloboClima-Users";

    public UserRepository(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
        _table = Table.LoadTable(_dynamoDbClient, TableName);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        try
        {
            var document = await _table.GetItemAsync(id);
            return document != null ? JsonSerializer.Deserialize<User>(document.ToJson()) : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("Email", ScanOperator.Equal, email);
            
            var search = _table.Scan(scanFilter);
            var documents = await search.GetNextSetAsync();
            
            var document = documents.FirstOrDefault();
            return document != null ? JsonSerializer.Deserialize<User>(document.ToJson()) : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        try
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("Username", ScanOperator.Equal, username);
            
            var search = _table.Scan(scanFilter);
            var documents = await search.GetNextSetAsync();
            
            var document = documents.FirstOrDefault();
            return document != null ? JsonSerializer.Deserialize<User>(document.ToJson()) : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<User> CreateAsync(User user)
    {
        var document = Document.FromJson(JsonSerializer.Serialize(user));
        await _table.PutItemAsync(document);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        var document = Document.FromJson(JsonSerializer.Serialize(user));
        await _table.PutItemAsync(document);
        return user;
    }

    public async Task DeleteAsync(string id)
    {
        await _table.DeleteItemAsync(id);
    }
}
