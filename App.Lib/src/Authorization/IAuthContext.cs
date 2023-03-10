namespace App.Lib.Authorization;

public interface IAuthContext
{
    public Guid CurrentTenant { get; }
}