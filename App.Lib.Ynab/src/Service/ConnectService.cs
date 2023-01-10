using App.Lib.Database;
using App.Lib.Net.Http;
using App.Lib.Ynab.Dto;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest.Dto;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace App.Lib.Ynab.Rest;

public class ConnectService : IConnectService
{
    public const string TokenName = "YNAB";

    private readonly IOAuthTokenStorage _tokenStorage;
    private readonly IOptions<YnabOptions> _options;
    private readonly HttpClient _httpClient;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly ILogger<ConnectService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ConnectService(
        IOAuthTokenStorage tokenStorage,
        IOptions<YnabOptions> options,
        HttpClient httpClient,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        ILogger<ConnectService> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _tokenStorage = tokenStorage;
        _options = options;
        _httpClient = httpClient;
        _urlHelperFactory = urlHelperFactory;
        _actionContextAccessor = actionContextAccessor;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<IOAuthToken> GetValidAccessToken()
    {
        var token = await _tokenStorage.Get(TokenName);
        if (
            string.IsNullOrEmpty((string)token.AccessToken) ||
            string.IsNullOrEmpty((string)token.RefreshToken))
        {
            _logger.LogTrace("GetValidAccessToken failed, no previous refresh or access token set");
            throw new TokenNotSetException();
        }

        if (!TokenIsExpired(token)) return token;

        return await FetchToken(new TokenRequestParams
        {
            ClientId = _options.Value.ClientId,
            ClientSecret = _options.Value.ClientSecret,
            RedirectUri = GetReturnUrl(),
            GrantType = "refresh_token",
            RefreshToken = token.RefreshToken
        });

    }

    public string GetRedirectUrl()
    {
        return _options.Value.AppAddress
            .AppendPathSegment("/oauth/authorize")
            .SetQueryParams(new
            {
                response_type = "code",
                scope = "read-only",
                client_id = _options.Value.ClientId,
                redirect_uri = GetReturnUrl(),
            });
    }

    public async Task ProcessReturn(string code)
    {
        await FetchToken(new TokenRequestParams
        {
            ClientId = _options.Value.ClientId,
            ClientSecret = _options.Value.ClientSecret,
            RedirectUri = GetReturnUrl(),
            GrantType = "authorization_code",
            Code = code
        });
    }

    public async Task<bool> IsConnected()
    {
        var token = await _tokenStorage.Get(TokenName);
        return !string.IsNullOrEmpty(token.AccessToken) && !TokenIsExpired(token);
    }

    private string GetReturnUrl()
    {
        var actionContext = _actionContextAccessor.ActionContext;
        var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

        return urlHelper.Action("Return", "Return", new { }, actionContext.HttpContext.Request.Scheme);
    }

    private async Task<IOAuthToken> StoreToken(TokenResponse token)
    {
        var newToken = new OAuthToken
        {
            Name = TokenName,
            RefreshToken = EncryptedString.FromDecryptedValue(token.RefreshToken),
            AccessToken = EncryptedString.FromDecryptedValue(token.AccessToken),
            ExpiresAt = _dateTimeProvider.UtcNow.AddSeconds(token.ExpiresIn),
        };
        await _tokenStorage.Store(newToken);
        return newToken;
    }

    private async Task<IOAuthToken> FetchToken(TokenRequestParams requestParams)
    {
        var tokenUrl = _options.Value.AppAddress.AppendPathSegment("/oauth/token");
        TokenResponse newToken;

        try
        {
            var tokenResponse = await _httpClient.PostAsJsonAsync(tokenUrl, requestParams);
            tokenResponse.EnsureSuccessStatusCode();

            newToken = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>();
        }
        catch (HttpRequestException exception)
        {
            throw CreateFetchTokenException(requestParams, exception);
        }
        catch (JsonException exception)
        {
            throw CreateFetchTokenException(requestParams, exception);
        }
        catch (JsonReaderException exception)
        {
            throw CreateFetchTokenException(requestParams, exception);
        }

        if (newToken == null || string.IsNullOrWhiteSpace(newToken.AccessToken) || string.IsNullOrWhiteSpace(newToken.RefreshToken))
        {
            throw new TokenException("Could not retrieve new access token.");
        }

        return await StoreToken(newToken);
    }

    private TokenException CreateFetchTokenException(TokenRequestParams requestParams, System.Exception exception)
    {
        _logger.LogWarning(
            exception,
            "Failed fetching new token with client_id: {client_id}, redirect_uri: {redirect_uri}, type: {type}",
            requestParams.ClientId,
            requestParams.RedirectUri,
            requestParams.GrantType);
        return new TokenException(exception.Message, exception);
    }

    private bool TokenIsExpired(IOAuthToken token)
    {
        return token.ExpiresAt < _dateTimeProvider.UtcNow;
    }
}