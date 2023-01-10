namespace App.Lib.Database;

public interface IOAuthTokenStorage
{
    public Task<IOAuthToken> Get(string name);
    public Task Store(IOAuthToken token);
}