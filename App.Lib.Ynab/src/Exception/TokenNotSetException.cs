using App.Lib.Ynab.Exception;

namespace App.Lib.Ynab;

public class TokenNotSetException : TokenException
{
    public TokenNotSetException(string message) : base(message)
    {
    }

    public TokenNotSetException() : base("YNAB access token not set.")
    {

    }
}