namespace App.Lib.Ynab.Exception;

public class TokenException : System.Exception
{
    public TokenException(string message) : base(message)
    { }

    public TokenException(string message, System.Exception? innerException) : base(message, innerException)
    { }
}