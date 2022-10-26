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

public class TokenInvalidException : TokenException
{
    public TokenInvalidException(string message) : base(message)
    {
    }

    public TokenInvalidException() : base("YNAB token invalid")
    {

    }
}