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
		
	}

	[Fact]
	public async void IsConnectedEmptyToken()
	{
		
	}
	
	[Fact]
	public async void IsConnectedExpiredToken()
	{
		
	}

	[Fact]
	public async void GetRedirectUrl()
	{
		
	}

	[Fact]
	public async void GetValidAccessToken()
	{

	}

	[Fact]
	public async void GetValidAccessTokenEmptyToken()
	{
		
	}
	
	[Fact]
	public async void GetValidAccessTokenExpired()
	{
		
	}

	[Fact]
	public async void GetValidAccessRefreshHttpException()
	{
		
	}

	[Fact]
	public async void GetValidAccessRefreshJsonException()
	{

	}

	[Fact]
	public async void ProcessReturn()
	{

	}
}