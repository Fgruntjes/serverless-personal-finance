using App.LibDatabase;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace App.TestsLib;

public class IntegrationTestFixture<TEntryPoint> :
    IDisposable,
    IClassFixture<TestApplicationFactory<TEntryPoint>> where TEntryPoint : class
{
    protected readonly TestApplicationFactory<TEntryPoint> _factory;
    protected readonly HttpClient _client;

    public IntegrationTestFixture(TestApplicationFactory<TEntryPoint> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public void Dispose()
    {
        ClearDatabase();
    }

    private void ClearDatabase()
    {
        var dbContext = _factory.Services.GetService<DbContext>();
        if (dbContext == null)
        {
            return;
        }

        var database = dbContext.Database;
        var filter = Builders<BsonDocument>.Filter.Not(Builders<BsonDocument>.Filter.Eq("_id", BsonNull.Value));
        var databaseCollections = database.ListCollectionNames().ToEnumerable();
        foreach (var name in databaseCollections)
        {
            database.GetCollection<BsonDocument>(name).DeleteMany(filter);
        }
    }
}