using System.Collections.Specialized;
using System.Net;
using App.Lib.Database;
using App.Lib.Tests.Logging;
using App.Lib.Ynab.Dto;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest;
using App.Lib.Ynab.Rest.Dto;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit.Abstractions;

namespace App.Lib.Ynab.Tests.Service;

public class ConnectServiceTest
{
    private readonly DateTime _now = new(2023, 01, 01);
    private readonly DateTime _validTokenDate = new(2023, 02, 02);
    private readonly DateTime _expiredTokenDate = new(2022, 12, 30);
    private readonly ConnectService _connectService;
    private readonly Mock<IOAuthTokenStorage> _tokenStorageMock;

    private readonly HttpClient _httpClient;
    private readonly WireMockServer _apiServer;
    private readonly WireMockServer _appServer;
    private readonly YnabOptions _ynabOptions;

    public ConnectServiceTest(
        ITestOutputHelper testOutputHelper)
    {
        _tokenStorageMock = new Mock<IOAuthTokenStorage>();
        _apiServer = WireMockServer.Start();
        _appServer = WireMockServer.Start();
        _httpClient = new HttpClient();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        dateTimeProviderMock
            .SetupGet(d => d.UtcNow)
            .Returns(_now);

        _ynabOptions = new YnabOptions()
        {
            ClientId = "SomeClientId",
            ClientSecret = "SomeClientSecret",
            ApiAddress = _apiServer.Url,
            AppAddress = _appServer.Url,
        };
        _connectService = new ConnectService(
            _tokenStorageMock.Object,
            Options.Create(_ynabOptions),
            _httpClient,
            XUnitLogger.CreateLogger<ConnectService>(testOutputHelper),
            dateTimeProviderMock.Object);
    }

    [Fact]
    public async void IsConnected_ValidToken()
    {
        SetupToken(new OAuthToken
        {
            Name = ConnectService.TokenName,
            AccessToken = EncryptedString.FromDecryptedValue("SomeToken"),
            ExpiresAt = _validTokenDate
        });

        (await _connectService.IsConnected())
            .Should()
            .BeTrue();
    }

    [Fact]
    public async void IsConnected_EmptyToken()
    {
        SetupToken(new OAuthToken
        {
            Name = ConnectService.TokenName,
        });

        (await _connectService.IsConnected())
            .Should()
            .BeFalse();
    }

    [Fact]
    public async void IsConnected_ExpiredToken()
    {
        SetupExpiredToken();

        (await _connectService.IsConnected())
            .Should()
            .BeFalse();
    }

    [Fact]
    public void GetRedirectUrl_Success()
    {
        var queryParams = new Uri(_connectService.GetRedirectUrl("https://someurl.com")).ParseQueryString();
        queryParams.Should()
            .BeEquivalentTo(new NameValueCollection
            {
                { "client_id", _ynabOptions.ClientId },
                { "response_type", "code" },
                { "scope", "read-only" },
                { "redirect_uri", "https://someurl.com" }
            });
    }

    [Fact]
    public async void GetValidAccessToken_Success()
    {
        var createdToken = SetupToken(new OAuthToken
        {
            Name = ConnectService.TokenName,
            AccessToken = EncryptedString.FromDecryptedValue("SomeAccessToken"),
            RefreshToken = EncryptedString.FromDecryptedValue("SomeRefreshToken"),
            ExpiresAt = _validTokenDate
        });

        (await _connectService.GetValidAccessToken())
            .Should()
            .BeEquivalentTo(createdToken);
    }

    [Fact]
    public async void GetValidAccess_AccessTokenEmptyToken()
    {
        SetupToken(new OAuthToken
        {
            Name = ConnectService.TokenName,
            RefreshToken = EncryptedString.FromDecryptedValue("SomeRefreshToken"),
            ExpiresAt = _validTokenDate
        });

        Func<Task> act = async () => { await _connectService.GetValidAccessToken(); };
        await act
            .Should()
            .ThrowAsync<TokenNotSetException>();
    }

    [Fact]
    public async void GetValidAccess_RefreshTokenEmptyToken()
    {
        SetupToken(new OAuthToken
        {
            Name = ConnectService.TokenName,
            AccessToken = EncryptedString.FromDecryptedValue("SomeAccessToken"),
            ExpiresAt = _validTokenDate
        });

        Func<Task> act = async () => { await _connectService.GetValidAccessToken(); };
        await act
            .Should()
            .ThrowAsync<TokenNotSetException>();
    }

    [Fact]
    public async void GetValidAccessToken_Expired()
    {
        SetupExpiredToken();

        _appServer
            .Given(Request.Create()
                .WithPath("/oauth/token")
                .WithBody(body =>
                {
                    var token = JsonConvert.DeserializeObject<TokenRequestParams>(body);
                    return
                        token.ClientId == _ynabOptions.ClientId &&
                        token.ClientSecret == _ynabOptions.ClientSecret &&
                        token.GrantType == "refresh_token" &&
                        token.RefreshToken == "SomeRefreshToken";
                }))
            .RespondWith(Response.Create().WithBodyAsJson(new TokenResponse
            {
                AccessToken = "OtherAccessToken",
                RefreshToken = "OtherRefreshToken",
                ExpiresIn = 600
            }));

        (await _connectService.GetValidAccessToken())
            .Should()
            .BeEquivalentTo(new OAuthToken()
            {
                Name = ConnectService.TokenName,
                AccessToken = EncryptedString.FromDecryptedValue("OtherAccessToken"),
                RefreshToken = EncryptedString.FromDecryptedValue("OtherRefreshToken"),
                ExpiresAt = _now.AddSeconds(600)
            });
    }

    [Fact]
    public async void GetValidAccess_RefreshHttpException()
    {
        SetupExpiredToken();

        _appServer
            .Given(Request.Create().WithPath("/oauth/token"))
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.InternalServerError));

        Func<Task> act = async () => { await _connectService.GetValidAccessToken(); };
        await act
            .Should()
            .ThrowAsync<TokenException>()
            .WithMessage("Response status code does not indicate success: *");
    }

    [Fact]
    public async void GetValidAccess_RefreshJsonException()
    {
        SetupExpiredToken();

        _appServer
            .Given(Request.Create().WithPath("/oauth/token"))
            .RespondWith(Response.Create()
                .WithBody("SomeInvalidJson")
                .WithHeader("content-encoding", "application/json"));

        Func<Task> act = async () => { await _connectService.GetValidAccessToken(); };
        await act
            .Should()
            .ThrowAsync<TokenException>()
            .WithMessage("Unexpected character encountered while parsing value: *");
    }

    [Fact]
    public async void ProcessReturn_Success()
    {
        _appServer
            .Given(Request.Create()
                .WithPath("/oauth/token")
                .WithBody(body =>
                {
                    var token = JsonConvert.DeserializeObject<TokenRequestParams>(body);
                    return
                        token.RedirectUri == "https://someurl.com" &&
                        token.ClientId == _ynabOptions.ClientId &&
                        token.ClientSecret == _ynabOptions.ClientSecret &&
                        token.GrantType == "authorization_code" &&
                        token.Code == "SomeCode";
                }))
            .RespondWith(Response.Create().WithBodyAsJson(new TokenResponse
            {
                AccessToken = "OtherAccessToken",
                RefreshToken = "OtherRefreshToken",
                ExpiresIn = 600
            }));

        await _connectService.ProcessReturn("SomeCode", "https://someurl.com");

        _tokenStorageMock
            .Verify(s =>
                s.Store(It.Is<IOAuthToken>(token =>
                    (string)token.AccessToken == "OtherAccessToken" &&
                    (string)token.RefreshToken == "OtherRefreshToken" &&
                    token.ExpiresAt == _now.AddSeconds(600)
                )),
                Times.Once());
    }

    [Fact]
    public async void Disconnect()
    {
        await _connectService.Disconnect();
        _tokenStorageMock.Verify(s => s.Delete(ConnectService.TokenName), Times.Once());
    }

    private IOAuthToken SetupExpiredToken()
    {
        return SetupToken(new OAuthToken
        {
            Name = ConnectService.TokenName,
            AccessToken = EncryptedString.FromDecryptedValue("SomeAccessToken"),
            RefreshToken = EncryptedString.FromDecryptedValue("SomeRefreshToken"),
            ExpiresAt = _expiredTokenDate
        });
    }

    private IOAuthToken SetupToken(IOAuthToken token)
    {
        _tokenStorageMock
            .Setup(s => s.Get(It.IsAny<string>()))
            .Returns(Task.FromResult(token));

        return token;
    }
}