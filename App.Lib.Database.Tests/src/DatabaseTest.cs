using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace App.Lib.Database.Tests;

public class DatabaseTest : IDisposable
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;
    protected readonly DatabaseContext _databaseContext;

    public DatabaseTest()
    {
        _databaseOptions = Options.Create(new DatabaseOptions
        {
            ConnectionString = "mongodb://username:password@127.0.0.1:27017",
            DatabaseName = $"test_db_{Guid.NewGuid()}"
        });
        _databaseContext = new DatabaseContext(_databaseOptions);
    }

    public void Dispose()
    {
        var client = new MongoClient(_databaseOptions.Value.ConnectionString);
        client.DropDatabase(_databaseOptions.Value.DatabaseName);
    }
}