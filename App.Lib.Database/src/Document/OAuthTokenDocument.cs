using MongoDB.Bson;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace App.Lib.Database.Document;

public class OAuthTokenDocument : IOAuthToken
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public Guid Tenant { get; set; }
    public EncryptedString? RefreshToken { get; set; }
    public EncryptedString? AccessToken { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public DateTime? LockedUntil { get; set; }

    public OAuthTokenDocument(string name, Guid tenant)
    {
        Name = name;
        Tenant = tenant;
    }
}