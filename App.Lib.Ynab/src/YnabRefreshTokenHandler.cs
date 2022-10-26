using System.Net.Http.Json;
using App.Lib.Database;
using App.Lib.Database.Document;

namespace App.Lib.Ynab;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

public class YnabRefreshTokenHandler : DelegatingHandler
{
    private const string TokenName = "YNAB";

    private readonly OAuthTokenStorage _tokenStorage;
    private readonly IOptions<YnabOptions> _options;

    public YnabRefreshTokenHandler(OAuthTokenStorage tokenStorage, IOptions<YnabOptions> options)
    {
        _tokenStorage = tokenStorage;
        _options = options;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var token = await _tokenStorage.Get(TokenName);
        if (token.AccessToken == null)
        {
            throw new TokenNotSetException("Can not use http client before AccessToken is set");
        }

        if (token.ExpiresAt < DateTime.UtcNow)
        {
            await RefreshToken(token);
        }

        request.Headers.Add("Authorization", "BEARER " + token.AccessToken);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task RefreshToken(OAuthTokenDocument token)
    {
        if (token.RefreshToken == null)
        {
            throw new TokenException("Can not refresh token, RefreshToken token set.");
        }

        var tokenResponse = await base.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString("/oauth/token", new Dictionary<string, string?>
            {
                ["client_id"] = _options.Value.ClientId,
                ["client_secret"] = _options.Value.ClientSecret,
                ["refresh_token"] = token.RefreshToken,
                ["grant_type"] = "refresh_token"
            })),
            CancellationToken.None);

        var newToken = await tokenResponse.Content.ReadFromJsonAsync<YnabTokenResponse>();
        if (newToken == null)
        {
            throw new TokenException("Could not retrieve new access token.");
        }

        token.AccessToken = EncryptedString.FromDecryptedValue(newToken.AccessToken);
        token.RefreshToken = EncryptedString.FromDecryptedValue(newToken.RefreshToken);
        token.ExpiresAt = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn);
        await _tokenStorage.Store(token);
    }
}