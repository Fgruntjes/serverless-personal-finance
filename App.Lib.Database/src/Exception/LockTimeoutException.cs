namespace App.Lib.Database.Exception;

public class LockTimeoutException : LockException
{
    public LockTimeoutException(
        string? message = null,
        System.Exception? innerException = null) : base(message, innerException)
    { }
}