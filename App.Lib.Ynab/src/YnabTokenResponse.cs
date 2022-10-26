namespace App.Lib.Ynab;

public class YnabTokenResponse
{
    public string AccessToken => null!;
    public string TokenType => null!;
    public int ExpiresIn { get; }
    public string RefreshToken => null!;
}