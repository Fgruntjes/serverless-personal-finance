using System.Net;
using App.Lib.Tests.Http;
using App.Lib.Ynab.Rest;
using Moq;

namespace App.Lib.Ynab.Tests.Rest;

public class TokenCleanupHandlerTest
{
    private readonly Mock<IConnectService> _connectServiceMock;

    public TokenCleanupHandlerTest()
    {
        _connectServiceMock = new Mock<IConnectService>();
    }

    [Fact]
    public async void NothingOnSuccess()
    {
        await InvokeHandler(new TestHandler());

        _connectServiceMock.Verify(s => s.Disconnect(), Times.Never());
    }

    [Fact]
    public async void NothingOnServerError()
    {
        await InvokeHandler(new TestHandler((_, cancellationToken) => Task.Factory.StartNew(
            () => new HttpResponseMessage(HttpStatusCode.InternalServerError), cancellationToken)));

        _connectServiceMock.Verify(s => s.Disconnect(), Times.Never());
    }

    [Fact]
    public async void DisconnectNotAuthorized()
    {
        await InvokeHandler(new TestHandler((_, cancellationToken) => Task.Factory.StartNew(
            () => new HttpResponseMessage(HttpStatusCode.Unauthorized), cancellationToken)));

        _connectServiceMock.Verify(s => s.Disconnect(), Times.Never());
    }

    private async Task InvokeHandler(DelegatingHandler testHandler)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");
        var invoker = new HttpMessageInvoker(new TokenCleanupHandler(_connectServiceMock.Object)
        {
            InnerHandler = testHandler
        });
        await invoker.SendAsync(httpRequestMessage, new CancellationToken());
    }
}