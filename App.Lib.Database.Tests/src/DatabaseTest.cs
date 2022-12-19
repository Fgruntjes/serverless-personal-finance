using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace App.Lib.Database.Tests;

public class DatabaseTest
{
	private static bool _serializerIsRegistered;
	private readonly IOptions<DatabaseOptions> _databaseOptions;
	protected readonly DatabaseContext _databaseContext;

	public DatabaseTest()
	{
		if (!_serializerIsRegistered)
		{
			BsonSerializer.RegisterSerializer(
				typeof(EncryptedString),
				new EncryptedStringSerializer(new EphemeralDataProtectionProvider()));
			_serializerIsRegistered = true;
		}
		
		
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