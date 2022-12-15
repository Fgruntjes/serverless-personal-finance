namespace App.Lib.Ynab.Dto;

public class TokenResponse
{
    public string AccessToken => null!;
    public string TokenType => null!;
    public int ExpiresIn { get; }
    public string RefreshToken => null!;
}