namespace App.Lib.Database.Exception;

public class LockException : System.Exception
{
    public LockException(
            string? message = null,
            System.Exception? innerException = null) : base(message, innerException)
    { }
}