using App.Lib.Ynab.Exception;

namespace App.Lib.Ynab.Rest;

public class RefreshTokenHandler : DelegatingHandler
{
    private readonly IConnectService _connectService;

    public RefreshTokenHandler(IConnectService connectService)
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
            var token = await _connectService.GetValidAccessToken();
            request.Headers.Add("Authorization", "BEARER " + token.AccessToken);
        }
        catch (TokenException)
        {}
        
        return await base.SendAsync(request, cancellationToken);
    }
}