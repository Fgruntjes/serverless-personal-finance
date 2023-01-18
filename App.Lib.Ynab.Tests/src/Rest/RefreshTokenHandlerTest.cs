using App.Lib.Database;
using App.Lib.Tests.Http;
using App.Lib.Ynab.Exception;
using App.Lib.Ynab.Rest;
using Moq;

namespace App.Lib.Ynab.Tests.Rest;

public class RefreshTokenHandlerTest
{
    private readonly Mock<IConnectService> _connectServiceMock;

    public RefreshTokenHandlerTest()
    {
        _connectServiceMock = new Mock<IConnectService>();
    }

    [Fact]
    public async void ValidTokenWithHeader()
    {
        _connectServiceMock.Setup(s => s.GetValidAccessToken())
            .ReturnsAsync(new OAuthToken
            {
                AccessToken = EncryptedString.FromDecryptedValue("sometoken")
            });

        var testHandler = await InvokeHandler();
        testHandler.Requests.Should().HaveCount(1);
        testHandler.Requests
            .First()
            .Headers
            .GetValues("Authorization")
            .First()
            .Should()
            .Be("BEARER sometoken");
    }

    [Fact]
    public async void IgnoreTokenException()
    {
        _connectServiceMock.Setup(s => s.GetValidAccessToken())
            .Throws(new TokenException("Some token error"));

        var testHandler = await InvokeHandler();
        testHandler.Requests.Should().HaveCount(1);
        testHandler.Requests
            .First()
            .Headers
            .Contains("Authorization")
            .Should()
            .BeFalse();
    }

    private async Task<TestHandler> InvokeHandler()
    {
        var testHandler = new TestHandler();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");
        var invoker = new HttpMessageInvoker(new RefreshTokenHandler(_connectServiceMock.Object)
        {
            InnerHandler = testHandler
        });
        await invoker.SendAsync(httpRequestMessage, new CancellationToken());

        return testHandler;
    }
}