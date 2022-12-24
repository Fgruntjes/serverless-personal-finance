using MongoDB.Bson;

namespace App.Lib.Database.Document;

public class OAuthTokenDocument : IOAuthToken
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public EncryptedString? RefreshToken { get; set; }
    public EncryptedString? AccessToken { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public DateTime? LockedUntil { get; set; }

    public OAuthTokenDocument(string name)
    {
        Name = name;
    }
}