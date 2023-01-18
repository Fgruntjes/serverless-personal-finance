using App.Lib.Database;

namespace App.Lib.Ynab;

public interface IConnectService
{
    public string GetRedirectUrl(string returnUrl);
    public Task<IOAuthToken> GetValidAccessToken();
    public Task ProcessReturn(string code, string returnUrl);
    public Task<bool> IsConnected();
    public Task Disconnect();
}