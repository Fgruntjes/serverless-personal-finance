using MongoDB.Bson;

namespace App.Lib.Database.Document;

public class OAuthTokenDocument
{
    public ObjectId Id { get; set; }
    public string Name { get; }
    public EncryptedString? RefreshToken { get; set; }
    public EncryptedString? AccessToken { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public OAuthTokenDocument(string name)
    {
        Name = name;
    }
}