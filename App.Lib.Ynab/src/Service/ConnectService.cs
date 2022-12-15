using System.Net.Http.Json;
using App.Lib.Database;
using App.Lib.Ynab.Dto;
using App.Lib.Ynab.Exception;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

namespace App.Lib.Ynab.Rest;

public class ConnectService : IConnectService
{
	private const string TokenName = "YNAB";
	
	private readonly OAuthTokenStorage _tokenStorage;
	private readonly IOptions<YnabOptions> _options;
	private readonly HttpClient _httpClient;
	private readonly IUrlHelperFactory _urlHelperFactory;
	private readonly IActionContextAccessor _actionContextAccessor;

	public ConnectService(
		OAuthTokenStorage tokenStorage,
		IOptions<YnabOptions> options,
		HttpClient httpClient,
		IUrlHelperFactory urlHelperFactory,
		IActionContextAccessor actionContextAccessor)
	{
		_tokenStorage = tokenStorage;
		_options = options;
		_httpClient = httpClient;
		_urlHelperFactory = urlHelperFactory;
		_actionContextAccessor = actionContextAccessor;
	}

	public async Task<IOAuthToken> GetValidAccessToken()
	{
		var token = await _tokenStorage.Get(TokenName);
		if (string.IsNullOrEmpty((string) token.AccessToken))
		{
			throw new TokenNotSetException();
		}

		if (TokenIsExpired(token))
		{
			await RefreshToken(token);
		}

		return token;
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
		FetchToken(new
		{
			client_id = _options.Value.ClientId,
			client_secret = _options.Value.ClientSecret,
			redirect_uri = GetReturnUrl(),
			grant_type = "authorization_code",
			code
		});
	}

	public async Task<bool> IsConnected()
	{
		var token = await _tokenStorage.Get(TokenName);
		return !string.IsNullOrEmpty(token.AccessToken) && !TokenIsExpired(token);
	}
	
	private async Task RefreshToken(IOAuthToken token)
	{
		FetchToken(new
		{
			client_id = _options.Value.ClientId,
			client_secret = _options.Value.ClientSecret,
			redirect_uri = GetReturnUrl(),
			refresh_token = token.RefreshToken,
			grant_type = "refresh_token",
		});
	}

	private object GetReturnUrl()
	{
		var actionContext = _actionContextAccessor.ActionContext;
		var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);
		
		return urlHelper.Action("Return", "Return", new { }, actionContext.HttpContext.Request.Scheme);
	}

	private async Task StoreToken(TokenResponse token)
	{
		var newToken = new OAuthToken
		{
			Name = TokenName,
			RefreshToken = EncryptedString.FromDecryptedValue(token.RefreshToken),
			AccessToken = EncryptedString.FromDecryptedValue(token.AccessToken),
			ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn),
		};
		await _tokenStorage.Store(newToken);
	}

	private async Task FetchToken<TRequestParams>(TRequestParams requestParams)
	{
		var tokenUrl = _options.Value.AppAddress.AppendPathSegment("/oauth/token");
		TokenResponse newToken;
		
		try
		{
			var tokenResponse = await _httpClient.PostAsJsonAsync(tokenUrl, requestParams);
			newToken = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>();
		}
		catch (HttpRequestException requestException)
		{
			throw new TokenException(requestException.Message, requestException);
		}
		
		if (newToken == null)
		{
			throw new TokenException("Could not retrieve new access token.");
		}
		
		await StoreToken(newToken);
	}

	private bool TokenIsExpired(IOAuthToken token)
	{
		return token.ExpiresAt < DateTime.UtcNow;
	}
}