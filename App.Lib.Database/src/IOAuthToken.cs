namespace App.Lib.Database;

public interface IOAuthToken
{
    public string Name { get; }
    public EncryptedString? RefreshToken { get; }
    public EncryptedString? AccessToken { get; }
    public DateTime? ExpiresAt { get; }
}