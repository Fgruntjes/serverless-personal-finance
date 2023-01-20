using System.Net;

namespace App.Lib.Ynab.Rest;

public class TokenCleanupHandler : DelegatingHandler
{
    private readonly IConnectService _connectService;

    public TokenCleanupHandler(IConnectService connectService)
    {
        _connectService = connectService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Refit.ApiException exception)
        {
            if (exception.StatusCode == HttpStatusCode.Unauthorized)
            {
                _connectService.Disconnect();
            }

            throw;
        }
    }
}