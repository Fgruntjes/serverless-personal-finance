using System.Net;
using App.Lib.Dto.Frontend;
using App.Lib.Tests;
using App.Lib.Ynab;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace App.Function.Integration.Ynab.Tests.Controller;

public class ConnectControllerTest : IntegrationTestFixture<Program>
{
    private readonly Mock<IConnectService> _mockedConnectService;

    public ConnectControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
        _mockedConnectService = new Mock<IConnectService>();
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(s => s.AddScoped(_ => _mockedConnectService.Object));
            })
            .CreateClient();
    }

    [Fact]
    public async void ConnectWhenNotConnected()
    {
        const string redirectUri = "https://fake.api/";
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(false);

        _mockedConnectService
            .Setup(c => c.GetRedirectUrl())
            .Returns(redirectUri);

        var response = await _client.GetAsync("/connect");
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        apiResponse
            .Should()
            .BeEquivalentTo(new ApiResponse<string>(redirectUri));
    }

    [Fact]
    public async void ConnectWhenConnected()
    {
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(true);

        var connectResponse = await _client.GetAsync("/connect");
        connectResponse
            .Should()
            .HaveStatusCode(HttpStatusCode.BadRequest);

        var apiResponse = await connectResponse.Content.ReadFromJsonAsync<ApiResponse<string>>();
        apiResponse.Should()
            .BeEquivalentTo(new ApiResponse<string>()
            {
                Errors = new[] { new AppApiError(ErrorType.BadRequest, "Already connected.") }
            });
    }
}