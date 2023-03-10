namespace App.Lib.Database;

public interface IOAuthTokenStorage
{
    public Task<IOAuthToken> Get(string name, Guid tenant);
    public Task Store(IOAuthToken token);
    public Task Delete(string name, Guid tenant);
}