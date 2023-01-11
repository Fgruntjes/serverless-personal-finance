using App.Lib.Database;

namespace App.Lib.Ynab;

public interface IConnectService
{
    public string GetRedirectUrl();
    public Task<IOAuthToken> GetValidAccessToken();
    public Task ProcessReturn(string code);
    public Task<bool> IsConnected();
    public Task Disconnect();
}