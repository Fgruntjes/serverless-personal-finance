using App.Lib.Database;
using Moq;

namespace App.Lib.Ynab.Tests.Service;

public class ConnectServiceTest
{
    private readonly Mock<OAuthTokenStorage> _tokenStorageMock;

    public ConnectServiceTest()
    {
        _tokenStorageMock = new Mock<OAuthTokenStorage>();
    }

    [Fact]
    public async void IsConnectedValidToken()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void IsConnectedEmptyToken()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void IsConnectedExpiredToken()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void GetRedirectUrl()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void GetValidAccessToken()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void GetValidAccessTokenEmptyToken()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void GetValidAccessTokenExpired()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void GetValidAccessRefreshHttpException()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void GetValidAccessRefreshJsonException()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async void ProcessReturn()
    {
        Assert.Fail("Not implemented");
    }
}