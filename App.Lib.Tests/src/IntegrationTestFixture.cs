using System.Net.Http.Headers;
using App.Lib.Database;
using App.Lib.Tests.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace App.Lib.Tests;

public class IntegrationTestFixture<TEntryPoint> :
    IDisposable,
    IClassFixture<TestApplicationFactory<TEntryPoint>> where TEntryPoint : class
{
    protected readonly TestApplicationFactory<TEntryPoint> _factory;
    protected HttpClient _client;

    public IntegrationTestFixture(TestApplicationFactory<TEntryPoint> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: TestAuthenticationHandler.TestScheme);
    }

    public void Dispose()
    {
        ClearDatabase();
    }

    private void ClearDatabase()
    {
        var dbContext = _factory.Services.GetService<DatabaseContext>();
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