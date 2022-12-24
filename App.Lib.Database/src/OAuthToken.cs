namespace App.Lib.Database;

public class OAuthToken : IOAuthToken
{
    public string Name { get; init; }
    public EncryptedString? RefreshToken { get; init; }
    public EncryptedString? AccessToken { get; init; }
    public DateTime? ExpiresAt { get; init; }
}