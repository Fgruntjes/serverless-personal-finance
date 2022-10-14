using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace App.LibDatabase;

public class DatabaseContext
{
    public IMongoDatabase Database { get; init; }

    public DatabaseContext(IOptions<DatabaseOptions> configuration)
    {
        var client = new MongoClient(configuration.Value.ConnectionString);
        Database = client.GetDatabase(configuration.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>()
    {
        var name = typeof(T).Name;
        name = Regex.Replace(name, @"Document$", String.Empty);
        return Database.GetCollection<T>(name);
    }
}