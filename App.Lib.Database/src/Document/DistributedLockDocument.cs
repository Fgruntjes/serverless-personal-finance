using MongoDB.Bson.Serialization.Attributes;

namespace App.Lib.Database.Document;

public class DistributedLockDocument
{
    [BsonId]
    public string Name { get; set; }

    public DateTime ExpiresAt { get; set; }
}