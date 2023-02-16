using System.Net;
using App.Lib.Dto.Frontend;
using App.Lib.Tests;
using Microsoft.AspNetCore.WebUtilities;
using Moq;

namespace App.Function.Integration.Ynab.Tests.Controller;

public class ConnectControllerTest : ControllerTest
{
    public ConnectControllerTest(TestApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void ConnectWhenNotConnected()
    {
        const string redirectUri = "https://fake.api/";
        _mockedConnectService
            .Setup(c => c.IsConnected())
            .ReturnsAsync(false);

        _mockedConnectService
            .Setup(c => c.GetRedirectUrl("http://localhost:3000/return"))
            .Returns(redirectUri);

        var response = await _client.GetAsync(QueryHelpers.AddQueryString("/connect", "returnUrl", "http://localhost:3000/return"));
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

        var connectResponse = await _client.GetAsync(QueryHelpers.AddQueryString("/connect", "returnUrl", "http://localhost:3000/return"));
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