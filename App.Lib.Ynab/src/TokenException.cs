namespace App.Lib.Ynab;

public class TokenException : Exception
{
    public TokenException(string message) : base(message)
    { }
}